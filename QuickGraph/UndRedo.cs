using System.Collections.Generic;
using System.Linq;

namespace ORM.RelationshipView
{
    public class UndRedo<TStep>
    {
        private readonly Stack<TStep> undoToggleStack = new Stack<TStep>(0);
        private readonly Stack<TStep> redoToggleStack = new Stack<TStep>(0);

        public bool CanUndoToggle => undoToggleStack.Any();
        public bool CanRedoToggle => redoToggleStack.Any();

        public TStep GetUndoStep()
        {
            return undoToggleStack.Pop();
        }

        public TStep GetRedoStep()
        {
            return redoToggleStack.Pop();
        }

        public void SetUndoStep(TStep step)
        {
            undoToggleStack.Push(step);
        }

        public void SetRedoStep(TStep step)
        {
            redoToggleStack.Push(step);
        }

        public void ClearRedoStack()
        {
            redoToggleStack.Clear();
        }
    }
}