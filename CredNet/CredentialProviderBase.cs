﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CredNet.Interop;

namespace CredNet
{
    public abstract class CredentialProviderBase : ICredentialProvider, ICredentialProviderSetUserArray, IDisposable
    {
        internal List<IntPtr> FieldDescriptorsPointers { get; set; } = new List<IntPtr>();
        internal bool Supported { get; set; }

        public List<ICredentialProviderUser> Users { get; internal set; } = new List<ICredentialProviderUser>();

        public UsageScenario UsageScenario { get; internal set; }

        public CredUIWinFlags UsageFlags { get; internal set; }

        public ICredentialProviderEvents Events { get; internal set; }
        public IntPtr AdviseContext { get; internal set; }

        public List<UserCredential> Credentials { get; set; } = new List<UserCredential>();
        public UserCredential DefaultCredential { get; set; }

        public abstract bool IsUsageSupported(UsageScenario usage, CredUIWinFlags dwFlags);
        public abstract void Initialize();

        public abstract void Dispose();

        public int SetUsageScenario(UsageScenario cpus, CredUIWinFlags flags)
        {
            UsageScenario = cpus;
            UsageFlags = flags;

            Trace.WriteLine($"UsageScenario: {cpus}, Flags: {flags}");
            Supported = IsUsageSupported(cpus, flags);
            return Supported ? HRESULT.S_OK : HRESULT.E_NOTIMPL;
        }

        public int SetSerialization(ref CredentialSerialization pcpcs)
        {
            return HRESULT.E_NOTIMPL;
        }

        public int Advise(ICredentialProviderEvents pcpe, IntPtr upAdviseContext)
        {
            Events = pcpe;
            AdviseContext = upAdviseContext;

            Initialize();

            Marshal.AddRef(Marshal.GetIUnknownForObject(Events));

            return HRESULT.S_OK;
        }

        public int UnAdvise()
        {
            Credentials.Clear();

            Dispose();

            if (Events != null)
            {
                Marshal.ReleaseComObject(Events);
                Events = null;
            }

            return HRESULT.S_OK;
        }

        public int GetFieldDescriptorCount(out uint pdwCount)
        {
            FieldDescriptorsPointers.Clear();

            foreach (var credential in Credentials)
            {
                foreach (var control in credential.Controls)
                {
                    var item = control.GetFieldDescriptor();

                    var size = Marshal.SizeOf<FieldDescriptor>();
                    var ptr = Marshal.AllocCoTaskMem(size);


                    Marshal.StructureToPtr<FieldDescriptor>(item, ptr, false);
                    FieldDescriptorsPointers.Add(ptr);
                }
            }

            pdwCount = (uint)FieldDescriptorsPointers.Count;
            return HRESULT.S_OK;
        }

        public int GetFieldDescriptorAt(uint dwIndex, out IntPtr ppcpfd)
        {
            ppcpfd = FieldDescriptorsPointers[(int)dwIndex];

            return HRESULT.S_OK;
        }

        public int GetCredentialCount(out uint pdwCount, out uint pdwDefault, out int pbAutoLogonWithDefault)
        {
            pdwCount = (uint)Credentials.Count;

            pdwDefault = unchecked((uint)Credentials.IndexOf(DefaultCredential));
            pbAutoLogonWithDefault = (int)pdwDefault >= 0 ? DefaultCredential.AutoLogon ? 1 : 0 : 0;

            return HRESULT.S_OK;
        }

        public int GetCredentialAt(uint dwIndex, out ICredentialProviderCredential ppcpc)
        {
            if (dwIndex >= Credentials.Count)
            {
                ppcpc = null;
                return HRESULT.E_INVALIDARG;
            }

            ppcpc = new CredentialProviderCredential(Credentials[(int)dwIndex], this);

            return HRESULT.S_OK;
        }

        public int SetUserArray(ICredentialProviderUserArray users)
        {
            Users.Clear();

            users.GetCount(out var count);

            for (uint i = 0; i < count; i++)
            {
                if (users.GetAt(i, out var user) == HRESULT.S_OK)
                {
                    Users.Add(user);
                }
            }

            return HRESULT.S_OK;
        }

        public int RaiseCredentialsChanged()
        {
            return Events.CredentialsChanged(AdviseContext);
        }
    }
}
