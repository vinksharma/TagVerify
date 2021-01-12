using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TagVerify
{
    public class NTagLibWrapper
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct URLPARSEDATA
        {
            public bool uEncryptedFound;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] uEncryptedData;

            public byte uPICCDataTag;

            public bool uIDFound;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] uID;

            public bool uReadCtrFound;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] uReadCtr;

            public bool uCMACFound;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] uCMACData;

            public bool uTamperFound;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] uTamperTag;

            public override string ToString()
            {
                return "uPICCDataTag = " + uPICCDataTag
                    + "  , uIDFound = " + uIDFound
                    + "  , uReadCtrFound = " + uReadCtrFound
                    + "  , uCMACFound = " + uCMACFound
                    + "  , uTamperFound = " + uTamperFound;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYDIVERSIFICATIONDATA
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] cDiversificationData;

            public Int16 nDiversificationDataLength;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] k_MasterKey;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] k_DiversifiedKey;
        }


        [DllImport("NTAGLib.dll", EntryPoint = "NTAG_Parse_URL", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool NTAG_Parse_URL(byte[] k_SDMMetaReadKey, byte[] sURL, short nURLLength, ref URLPARSEDATA uRetData);


        public class DecryptedNFcTag
        {
            public string UId;
            public string UnEncryptedData;
            public int ReadCounter;
            public bool IsTampered;

            public DecryptedNFcTag(URLPARSEDATA parsedData)
            {
                IsTampered = parsedData.uTamperFound;
                if (parsedData.uIDFound)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in parsedData.uID)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    UId = sb.ToString().ToUpper();
                }
                else
                {
                    UId = null;
                }

                if (parsedData.uEncryptedFound)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in parsedData.uEncryptedData)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    UnEncryptedData = sb.ToString().ToUpper();
                }
                else
                {
                    UnEncryptedData = null;
                }

                if (parsedData.uReadCtrFound)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in parsedData.uReadCtr)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    ReadCounter = int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    ReadCounter = -1;
                }
            }
        }

        // Removed unused structures and methods
        public static DecryptedNFcTag ParseTagUrl(string url)
        {
            URLPARSEDATA retData = new URLPARSEDATA();
            byte[] k_SDMMetaReadKey = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //string url = "https://www.ntag.coinworldplus.com/verify?enc=E9B9E00EE736B677669ACE601DE14965xt=76FAFCAF5ADD38D1D9AB1CBBB33F32C4x7CA7D37726EA214F";


            // Instead of adding + 2 to the length, added two null 0 chars to the end of array.
            // Also the method declaration of NTAG_Parse_URL has been changed and string is replaced by byte[]
            url = url.Replace("https://www.", "") + "\0\0";
            short nURLLength = (short)(url.Length);
            bool returnValue = NTAG_Parse_URL(k_SDMMetaReadKey, Encoding.ASCII.GetBytes(url), nURLLength, ref retData);

            DecryptedNFcTag tagData = new DecryptedNFcTag(retData);
            Console.WriteLine("NTAG_Parse_URL: " + retData.ToString());

            return tagData;
        }
    }
}
