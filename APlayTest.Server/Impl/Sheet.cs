/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using APlay.Common;
using APlay.Common.Utils;
using APlay.Common.DataTypes;
using APlayTest.Server;
using APlayTest.Services.Infracstructure;
using sbardos.UndoFramework;

namespace APlayTest.Server
{
    public class Sheet : APlayTest.Server.SheetSkeleton
    {
        private readonly IUndoService _undoService;

        /// <summary>
        /// Use this constructor to create instances in your code.
        /// Note: leave the APInitOb null. Aplay sets this object if initialized by aplay.
        ///  if you want to determine in the constructor if the object is user created or by aplay - check IsInitializedByAPlay
        /// </summary>
        public Sheet()
        {

        }



        public Sheet(IUndoService undoService)
        {
            _undoService = undoService;

            _undoService.ActiveStateChanged += UndoServiceOnActiveStateChanged;

        }

        private void UndoServiceOnActiveStateChanged(object sender, ActiveStateChangedEventArgs e)
        {
            foreach (var change in e.ChangeSet.Where(c => c.OwnerId == Id))
            {

                APlay.Common.Logging.Logger.LogDesigned(2,
                      "ActiveStateChanged received and updated state. OwnerId: " + change.OwnerId,
                      "AplayTest.Server.Sheet");

                if (e.ChangeDirection == StateChangeDirection.Undo)
                {
                    if (change.ChangeReason == ChangeReason.InsertAt)
                    {
                        BlockSymbols.RemoveAt(change.IndexAt);
                    }
                    else if (change.ChangeReason == ChangeReason.RemoveAt)
                    {
                        BlockSymbols.Insert(change.IndexAt,
                            new BlockSymbol((BlockSymbolUndoable)change.RedoObjectState, e.ChangeSet, _undoService));
                    }
                }
                else if (e.ChangeDirection == StateChangeDirection.Redo)
                {
                    if (change.ChangeReason == ChangeReason.InsertAt)
                    {
                        BlockSymbols.Insert(change.IndexAt,
                             new BlockSymbol((BlockSymbolUndoable)change.RedoObjectState, e.ChangeSet, _undoService));
                    }
                    else if (change.ChangeReason == ChangeReason.RemoveAt)
                    {
                        BlockSymbols.RemoveAt(change.IndexAt);
                    }
                }

                var storedObject = e.ChangeDirection == StateChangeDirection.Undo
                                     ? change.UndoObjectState as SheetUndoable
                                     : change.RedoObjectState as SheetUndoable;

                if (storedObject != null)
                {
                    if (change.ChangeReason == ChangeReason.Update)
                    {
                        Name = storedObject.Name;
                    }    
                }
                
            }
        }


        public override BlockSymbol onCreateBlockSymbol()
        {
            var blockSymbol = new BlockSymbol(_undoService) { Id = IdGenerator.GetNextId(), PositionX = 0, PositionY = 0 };
           
            return blockSymbol;
        }

        public override void onAdd(BlockSymbol blockSymbol__, Client client)
        {
            APlay.Common.Logging.Logger.LogDesigned(2, "Sheet.onAdd called", "AplayTest.Server.Sheet");

            var undoObject = new BlockSymbolUndoable(blockSymbol__);
            _undoService.AddInsert(Id, undoObject, BlockSymbols.Count, "Adding new Block", client.Id);

            BlockSymbols.Add(blockSymbol__);

        }

        public override void onRemove(BlockSymbol blockSymbol__, Client client__)
        {
            APlay.Common.Logging.Logger.LogDesigned(2, "Sheet.onRemove called", "AplayTest.Server.Sheet");

            var toBeDeleted = BlockSymbols.First(t => t.Id == blockSymbol__.Id);
            var index = BlockSymbols.IndexOf(toBeDeleted);

            var undoObject = new BlockSymbolUndoable(blockSymbol__);
            _undoService.AddRemove(Id, undoObject, index, "Removing Block [" + toBeDeleted.Id + "]", client__.Id);

            BlockSymbols.RemoveAt(index);
        }


        public override void onSetName(string name__, Client client__)
        {
            if (Name == name__)
                return;

            var oldState = new SheetUndoable(this);

            Name = name__;

            var newState = new SheetUndoable(this);

            _undoService.AddUpdate(oldState, newState, "Sheet name changed", client__.Id);
        }
    }

    public class SheetUndoable : IUndoable
    {
        public SheetUndoable(Sheet sheet)
        {
            Id = sheet.Id;
            Name = sheet.Name;
            BlockIds = sheet.BlockSymbols.Select(b => b.Id);
        }

        public IEnumerable<int> BlockIds { get; private set; }

        public string Name { get; set; }

        public int Id { get; private set; }

        public string Dump()
        {
            return "Name: " + Name;
        }
    }
}
