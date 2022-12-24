using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CredNet.Interop;

namespace CredNet.Controls
{
    public class SmallLabel : FieldControl
    {
        public override FieldType GetFieldType() => FieldType.SmallText;
    }

    public class BigLabel : FieldControl
    {
        public override FieldType GetFieldType() => FieldType.LargeText;
    }

    public class SmallLabelCredentialProviderLabel : FieldControl
    {
        public override Guid GetFieldTypeGuid() => FieldTypeGuids.CPFG_CREDENTIAL_PROVIDER_LABEL;

        public override FieldType GetFieldType() => FieldType.SmallText;
    }
}
