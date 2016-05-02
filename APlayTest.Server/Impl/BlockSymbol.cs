/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using APlay;
using APlay.Common;
using APlay.Common.Utils;
using APlay.Common.DataTypes;
using APlayTest.Server;
using sbardos.UndoFramework;

namespace APlayTest.Server
{
    public class BlockSymbol : APlayTest.Server.BlockSymbolSkeleton
    {
        private readonly IUndoService _undoService;

        /// <summary>
        /// Use this constructor to create instances in your code.
        /// Note: leave the APInitOb null. Aplay sets this object if initialized by aplay.
        ///  if you want to determine in the constructor if the object is user created or by aplay - check IsInitializedByAPlay
        /// </summary>

        public BlockSymbol() :base()
        {
            
        }

        public BlockSymbol(IUndoService undoService) :this()
        {
            _undoService = undoService;

            _undoService.ActiveStateChanged += UndoServiceOnActiveStateChanged;
        }

   
        public BlockSymbol(BlockSymbolUndoable undoable, ChangeSet changeSet, IUndoService undoService):this(undoService)
        {
            Id = undoable.Id;
            PositionX = undoable.Position.X;
            PositionY = undoable.Position.Y;

            foreach (var change in changeSet.Where(c => c.OwnerId == Id))
            {
                
            }
        }

        private void UndoServiceOnActiveStateChanged(object sender, ActiveStateChangedEventArgs e)
        {
            foreach (var change in e.ChangeSet.Where(c => c.OwnerId == Id))
            {
                var storedObject = e.ChangeDirection == StateChangeDirection.Undo
                  ? (BlockSymbolUndoable)change.UndoObjectState
                  : (BlockSymbolUndoable)change.RedoObjectState;
                switch (change.ChangeReason)
                {
                    case ChangeReason.InsertAt:
                        break;
                    case ChangeReason.Update:
                        PositionX = storedObject.Position.X;
                        PositionY = storedObject.Position.Y;

                        APlay.Common.Logging.Logger.LogDesigned(2,
                            "BlockSymbol: ActiveStateChanged received and updated state. OwnerId: " + change.OwnerId,
                            "Undo.Server.Task");
                        break;
                    case ChangeReason.RemoveAt:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        public override void onSetPosition(AplayPoint position__, Client client__)
        {
            if (Math.Abs(PositionX - position__.X) < double.Epsilon && Math.Abs(PositionY - position__.Y) < double.Epsilon)
            {
                return;
            }

            var oldState = new BlockSymbolUndoable(Id, PositionX, PositionY);

            PositionX = position__.X;
            PositionY = position__.Y;

            _undoService.AddUpdate(oldState, new BlockSymbolUndoable(this), "Position of block changed", client__.Id);
           
        }
    }

    public class BlockSymbolUndoable : IUndoable
    {
        public BlockSymbolUndoable(BlockSymbol blockSymbol)
        {
            Id = blockSymbol.Id;
            Position = new AplayPoint(blockSymbol.PositionX, blockSymbol.PositionY);
        }

        public BlockSymbolUndoable(int id, double positionX, double positionY)
        {
            Id = id;
            Position = new AplayPoint(positionX, positionY);
        }

        public AplayPoint Position { get; private set; }

        public int Id { get; private set; }

        public string Dump()
        {
            return "Block Id: " + Id + ", Pos: " + Position.ToString();
        }
    }
}
