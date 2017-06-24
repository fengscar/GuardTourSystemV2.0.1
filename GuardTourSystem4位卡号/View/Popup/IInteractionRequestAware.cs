using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuardTourSystem.View.Popup
{
    public interface IInteractionRequestAware
    {
        /// <summary>
        /// The <see cref="INotification"/> passed when the interaction request was raised.
        /// </summary>
        Notification Notification { get; set; }

        /// <summary>
        /// An <see cref="Action"/> that can be invoked to finish the interaction.
        /// </summary>
        Action FinishInteraction { get; set; }
    }
}
