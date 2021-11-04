using System;

namespace CrossoutLogView.GUI.Core
{
    public abstract class CollectionViewModelBase : ViewModelBase, ICollectionViewModel
    {
        public void UpdateCollectionsSafe()
        {
            try
            {
                UpdateCollections();
            }
            catch (Exception)
            {
            }
        }

        protected abstract void UpdateCollections();
    }
}