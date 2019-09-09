using System;

namespace PlusValuesFifo.Models.Equities
{
    public interface IEquitiesInputEvent : IInputEvent
    {

        /// <summary>
        /// <see cref="EquitiesInputEvent.AmountUsed"/>
        /// </summary>
        decimal AmountUsed { get; set; }
    }

    public interface IEquitiesOutputEvent : IOutputEvent
    {
        decimal Pmp { get; }
    }
}