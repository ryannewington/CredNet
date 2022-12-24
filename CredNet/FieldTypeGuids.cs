using System;

namespace CredNet
{
    internal static class FieldTypeGuids
    {
        public static Guid CPFG_LOGON_USERNAME = Guid.Parse("da15bbe8-954d-4fd3-b0f4-1fb5b90b174b");
        public static Guid CPFG_LOGON_PASSWORD = Guid.Parse("60624cfa-a477-47b1-8a8e-3a4a19981827");
        public static Guid CPFG_SMARTCARD_USERNAME = Guid.Parse("3e1ecf69-568c-4d96-9d59-46444174e2d6");
        public static Guid CPFG_SMARTCARD_PIN = Guid.Parse("4fe5263b-9181-46c1-b0a4-9dedd4db7dea");
        public static Guid CPFG_CREDENTIAL_PROVIDER_LOGO = Guid.Parse("2d837775-f6cd-464e-a745-482fd0b47493");
        public static Guid CPFG_CREDENTIAL_PROVIDER_LABEL = Guid.Parse("286bbff3-bad4-438f-b007-79b7267c3d48");
    }
}
