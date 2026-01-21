using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent: MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        public enum Mode
        {
            Bound,
            External
        }
    }
}