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
using APlayTest.Services.Infracstructure;
using sbardos.UndoFramework;
using Undo.Server;
namespace Undo.Server
{
    public class TaskManager : Undo.Server.TaskManagerSkeleton
    {
        private readonly IUndoService _undoService;

        /// <summary>
        /// Use this constructor to create instances in your code.
        /// Note: leave the APInitOb null. Aplay sets this object if initialized by aplay.
        ///  if you want to determine in the constructor if the object is user created or by aplay - check IsInitializedByAPlay
        /// </summary>
        public TaskManager()
        {
            Id = IdGenerator.GetNextId();
        }


        public TaskManager(IUndoService undoService)
            : base()
        {
            _undoService = undoService;
            _undoService.ActiveStateChanged += _undoService_ActiveStateChanged;
        }

        void _undoService_ActiveStateChanged(object sender, ActiveStateChangedEventArgs e)
        {
            
            foreach (var change in e.ChangeSet.Where(cs => cs.OwnerId == Id))
            {
                APlay.Common.Logging.Logger.LogDesigned(2,
                      "ActiveStateChanged received and updated state. OwnerId: " + change.OwnerId,
                      "Undo.Server.TaskManager");

                if (e.ChangeDirection == StateChangeDirection.Undo)
                {
                    if (change.ChangeReason == ChangeReason.InsertAt)
                    {
                        Tasks.RemoveAt(change.IndexAt);              
                    }
                    else if (change.ChangeReason == ChangeReason.RemoveAt)
                    {
                        var storedObject = (Task.UndoObject)change.RedoObjectState;
                        Tasks.Insert(change.IndexAt,
                            new Task(storedObject.Id, storedObject.IsDone, storedObject.Description, _undoService));
                    }
                }
                else if (e.ChangeDirection == StateChangeDirection.Redo)
                {
                    if (change.ChangeReason == ChangeReason.InsertAt)
                    {
                        var storedObject = (Task.UndoObject)change.RedoObjectState;
                    
                        Tasks.Insert(change.IndexAt,
                            new Task(storedObject.Id, storedObject.IsDone, storedObject.Description, _undoService));
                    }
                    else if (change.ChangeReason == ChangeReason.RemoveAt)
                    {
                        Tasks.RemoveAt(change.IndexAt);
                    }
                }
            }
        }



        public TaskManager(Task[] tasks, IUndoService undoService)
        {
            _undoService = undoService;
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }

        public override Undo.Server.Task onCreateTask(Undo.Server.Client client__)
        {
            // Autogenerated log message for call
            APlay.Common.Logging.Logger.LogDesigned(2, "TaskManager.onCreateTask called", "Undo.Server.TaskManager");

            var id = IdGenerator.GetNextId();

            return new Task(id,false, "Do something [" + id + "]", _undoService);
        }
        public override void onAddTask(Undo.Server.Task task__, Undo.Server.Client client__)
        {
            // Autogenerated log message for call
            APlay.Common.Logging.Logger.LogDesigned(2, "TaskManager.onAddTask called", "Undo.Server.TaskManager");

            _undoService.Add(new Change(ChangeReason.InsertAt, Id, task__.CreateUndoObject(), Tasks.Count), client__.Id);

            Tasks.Add(task__);
        }



        public override void onRemoveTask(int id__, Undo.Server.Client client__)
        {
            // Autogenerated log message for call
            APlay.Common.Logging.Logger.LogDesigned(2, "TaskManager.onRemoveTask called", "Undo.Server.TaskManager");

            var toBeDeleted = Tasks.First(t => t.Id == id__);
            var index = Tasks.IndexOf(toBeDeleted);

            _undoService.Add(new Change(ChangeReason.RemoveAt, Id, toBeDeleted.CreateUndoObject(), index), client__.Id);

            Tasks.RemoveAt(index);
        }

        private struct UndoObject : IUndoable
        {
            public UndoObject(int id)
                : this()
            {
                Id = id;
            }

            public int Id { get; private set; }
            public string Dump()
            {
                return String.Empty;
            }
        }
    }

}
