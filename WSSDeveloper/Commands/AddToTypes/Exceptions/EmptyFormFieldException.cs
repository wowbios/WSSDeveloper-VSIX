using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes
{
    internal class EmptyFormFieldException : Exception
    {
        public EmptyFormFieldException(NamedBox box)
            : base($"Не заполнено обязательное поле '{box.GroupBox.Text}'")
        {

        }
    }
}
