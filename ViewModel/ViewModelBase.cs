using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationEditor.ViewModel
{
    public class ViewModelBase : NotifyBase
    {
        public delegate void FiledModifiedEventHandler(object sender);
        public event FiledModifiedEventHandler OnFileModified;

        protected void MarkFileModified()
        {
            OnFileModified?.Invoke(this);
        }
    }
}
