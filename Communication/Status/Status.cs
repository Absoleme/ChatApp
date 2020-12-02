using System;
namespace Communication.Status
{
    [Serializable]
    public enum Status
    {
        Connected,
        Disconnected,
        Validated,
        InSession
    }
}
