﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CredNet.Interop;

namespace CredNet
{
    public abstract class CredentialProviderFilterBase : ICredentialProviderFilter
    {
        public abstract bool IsProviderAllowed(UsageScenario scenario, CredUI.Flags uiFlags, ref Guid clsIdProvider);

        public abstract ManagedCredentialSerialization UpdateRemoteCredential(
            ManagedCredentialSerialization inputSerialization);

        public unsafe int Filter(UsageScenario cpus, CredUIWinFlags dwFlags, IntPtr rgclsidProviders, IntPtr rgbAllow, uint cProviders)
        {
            Guid* pProviders = (Guid*) rgclsidProviders.ToPointer();
            bool* pAllow = (bool*) rgbAllow.ToPointer();

            try
            {
                for (uint i = 0; i < cProviders; i++)
                {
                    pAllow[i] = IsProviderAllowed(cpus, (CredUI.Flags) dwFlags, ref pProviders[i]);
                }
            }
            catch (NotImplementedException)
            {
                return HRESULT.E_NOTIMPL;
            }
            catch
            {
                return HRESULT.E_FAIL;
            }

            return HRESULT.S_OK;
        }

        public int UpdateRemoteCredential(ref CredentialSerialization pcpcsIn, ref CredentialSerialization pcpcsOut)
        {
            try
            {
                pcpcsOut = UpdateRemoteCredential(ManagedCredentialSerialization.FromNative(pcpcsIn)).ToNative();
            }
            catch (NotImplementedException)
            {
                return HRESULT.E_NOTIMPL;
            }
            catch
            {
                return HRESULT.E_FAIL;
            }

            return HRESULT.S_OK;
        }
    }
}
