/*
    Copyright 2015 MCGalaxy

    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    https://opensource.org/license/ecl-2-0/
    https://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
#if NET_20
using System;
using System.Collections.Generic;
using System.Threading;
namespace System.Runtime.CompilerServices {
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute {}
}

namespace Flames.Util {

    public sealed class IReaderWriterLock {
        
        public ReaderWriterLock locker = new ReaderWriterLock();

        public IDisposable AccquireRead() { return AccquireRead(int.MaxValue); }
        public IDisposable AccquireWrite() { return AccquireWrite(int.MaxValue); }
        
        public IDisposable AccquireRead(int msTimeout) {
            try {
                locker.AcquireReaderLock(msTimeout);
            } catch (ApplicationException) {
                return null;
            }
            return new SlimLock(locker, false);
        }

        public IDisposable AccquireWrite(int msTimeout) {
            try {
                locker.AcquireWriterLock(msTimeout);
            } catch (ApplicationException) {
                return null;
            }
            return new SlimLock(locker, true);
        }
        
        
        public class SlimLock : IDisposable {
            public ReaderWriterLock locker;
            public bool writeMode;
            
            public SlimLock(ReaderWriterLock locker, bool writeMode) {
                this.locker = locker;
                this.writeMode = writeMode;
            }
            
            public void Dispose() {
                if (writeMode) {
                    locker.ReleaseWriterLock();
                } else {
                    locker.ReleaseReaderLock();
                }
                locker = null;
            }
        }
    }
}
#endif