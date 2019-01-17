// Guids.cs
// MUST match guids.h
using System;

namespace WSSConsulting.WSSDeveloper
{
    /// <summary>
    /// Список глобальных идентификаторов.
    /// </summary>
    internal static class GuidList
    {
        /// <summary>
        /// Идентификатор пакета расширения.
        /// </summary>
        public const string guidWSSDeveloperPkgString = "5BAEFD29-0E52-4A7D-ABBD-D10944C72072";

        /// <summary>
        /// Идентификатор меню команд WSS.
        /// </summary>
        public const string WSSCommands = "4AA4737A-542F-452D-8550-C1EB389CFBAC";

        /// <summary>
        /// Идентификатор меню команд WSS.
        /// </summary>
        public static readonly Guid WSSCommandsGuid = new Guid(WSSCommands);
    };
}