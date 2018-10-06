using MetadataExtractor.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MetadataExtractor.Implementations {
    public class XmpReader : IDisposable {

        private const string _xmpLibPath = "XmpReader.dll";

        private IntPtr _xmpMetaHandle = IntPtr.Zero;

        private IntPtr _xmpIteratorHandle = IntPtr.Zero;

        private bool _disposed = false;

        [DllImport(_xmpLibPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetMeta([MarshalAs(UnmanagedType.LPWStr)]string filePath);

        [DllImport(_xmpLibPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetMetaFromXml(string xml);

        [DllImport(_xmpLibPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetIterator(IntPtr metaPtr);

        [DllImport(_xmpLibPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool Next(
            IntPtr iteratorPtr,
            out IntPtr schemaNS,
            out int schemaNSSize,
            out IntPtr propPath,
            out int propPathSize,
            out IntPtr propVal,
            out int propValSize);

        [DllImport(_xmpLibPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DestroyMeta(IntPtr ptr);

        [DllImport(_xmpLibPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DestroyIter(IntPtr ptr);

        [DllImport(_xmpLibPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DestroyString(IntPtr ptr);

        [DllImport(_xmpLibPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Terminate();

        private XmpReader() {
            // Default constructor is private. Use the static factory methods instead.
        }

        /// <summary>
        /// Inits XmpReader from media file.
        /// </summary>
        /// <exception cref="ArgumentNullException">File path provided is invalid: null or empty.</exception>
        /// <exception cref="InvalidOperationException">Initialization of handles failed in DLL.</exception>
        public static XmpReader InitFromMediaFile(string filePath) {

            if (string.IsNullOrWhiteSpace(filePath)) {
                throw new ArgumentNullException("filePath");
            }

            XmpReader instance = new XmpReader();
            string errorMsg = string.Empty;

            try {
                instance._xmpMetaHandle = GetMeta(filePath);
            }
            catch (Exception) {
                errorMsg = string.Format("Could not initialize XmpMeta from DLL for file {0}!", filePath);
                throw new InvalidOperationException(errorMsg);
            }

            if (instance._xmpMetaHandle == IntPtr.Zero) {
                errorMsg = string.Format("_xmpMetaHandle is <null> for file {0}!", filePath);
                throw new InvalidOperationException(errorMsg);
            }

            try {
                instance._xmpIteratorHandle = GetIterator(instance._xmpMetaHandle);
            }
            catch (Exception) {
                errorMsg = string.Format("Could not initialize XmpIterator from DLL for file {0}!", filePath);
                instance.Cleanup();
                throw new InvalidOperationException(errorMsg);
            }
            if (instance._xmpIteratorHandle == IntPtr.Zero) {
                errorMsg = string.Format("_xmpIteratorHandle is <null> for file {0}!", filePath);
                instance.Cleanup();
                throw new InvalidOperationException(errorMsg);
            }

            return instance;
        }

        /// <summary>
        /// Inits XmpReader from xml string.
        /// </summary>
        /// <exception cref="ArgumentNullException">XML string provided is invalid: null or empty.</exception>
        /// <exception cref="InvalidOperationException">Initialization of handles failed in DLL.</exception>
        public static XmpReader InitFromXml(string xml) {

            if (string.IsNullOrWhiteSpace(xml)) {
                throw new ArgumentNullException("xml");
            }

            XmpReader instance = new XmpReader();
            string errorMsg = string.Empty;

            try {
                instance._xmpMetaHandle = GetMetaFromXml(xml);
            }
            catch (Exception) {
                errorMsg = "Could not initialize XmpMeta from XML!";
                throw new InvalidOperationException(errorMsg);
            }

            if (instance._xmpMetaHandle == IntPtr.Zero) {
                errorMsg = "_xmpMetaHandle is <null> for XML!";
                throw new InvalidOperationException(errorMsg);
            }

            try {
                instance._xmpIteratorHandle = GetIterator(instance._xmpMetaHandle);
            }
            catch (Exception) {
                errorMsg = "Could not initialize XmpIterator from DLL XML!";
                instance.Cleanup();
                throw new InvalidOperationException(errorMsg);
            }
            if (instance._xmpIteratorHandle == IntPtr.Zero) {
                errorMsg = "_xmpIteratorHandle is <null> for XML!";
                instance.Cleanup();
                throw new InvalidOperationException(errorMsg);
            }
            return instance;
        }

        ~XmpReader() {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {

            if (_disposed) {
                return;
            }

            Cleanup();

            _disposed = true;
        }

        /// <summary>
        /// Dispose the class.
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }

        /// <summary>
        /// Destroys handles.
        /// </summary>
        private void Cleanup() {

            if (_xmpMetaHandle != IntPtr.Zero) {
                try {
                    DestroyMeta(_xmpMetaHandle);
                    _xmpMetaHandle = IntPtr.Zero;
                }
                catch (Exception) {
                }
            }
            if (_xmpIteratorHandle != IntPtr.Zero) {
                try {
                    DestroyIter(_xmpIteratorHandle);
                    _xmpIteratorHandle = IntPtr.Zero;
                }
                catch (Exception) {
                }
            }
        }

        /// <summary>
        /// Dumps XMP metadata as flat key-value structure
        /// </summary>
        public Dictionary<string, string> DumpFlat() {

            Dictionary<string, string> result = new Dictionary<string, string>();

            IntPtr schemaNs, propPath, propValue;
            int schemaNsLength, propPathLength, propValueLength;


            while (Next(
                _xmpIteratorHandle,
                out schemaNs, out schemaNsLength,
                out propPath, out propPathLength,
                out propValue, out propValueLength)
            ) {

                string prop = string.Empty;
                string val = string.Empty;

                if (propPathLength > 0 && propPath != IntPtr.Zero) {
                    prop = MarshalCopy(propPath, propPathLength);
                }

                if (propValueLength > 0 && propValue != IntPtr.Zero) {
                    val = MarshalCopy(propValue, propValueLength);
                }

                if (!string.IsNullOrWhiteSpace(prop) && !string.IsNullOrWhiteSpace(val)) {
                    result.Add(prop, val);
                }

            }

            return result;
        }

        /// <summary>
        /// Copies string from unmanaged handle and destroys the handle.
        /// </summary>
        private string MarshalCopy(IntPtr stringPointer, int stringLength) {

            if (stringPointer == IntPtr.Zero || stringLength == 0) {
                throw new ArgumentNullException("stringPointer or stringLength");
            }

            byte[] byteBuffer = new byte[stringLength];
            Marshal.Copy(stringPointer, byteBuffer, 0, stringLength);

            try {
                DestroyString(stringPointer);
            }
            catch (Exception) {
            }

            return new StreamReader(new MemoryStream(byteBuffer), true).ReadToEnd();

        }
    }
}
