#pragma once
#ifndef _NTAGLIB_H_
#define _NTAGLIB_H_

#ifdef NTAGLIB_EXPORTS
#define NTAGLIB_API __declspec(dllexport)
#else
#define NTAGLIB_API __declspec(dllimport)
#endif

typedef struct {
	BYTE	bFileOptions;
	WORD	wAccessRights;
	BYTE	bSDMFileOptions;
	WORD	wSDMAccessRights;
	unsigned char cENCPICCDataOffset[3];
	unsigned char cSDMMACOffset[3];
	unsigned char cSDMMACInputOffset[3];
} SDMFILESETTINGS;

typedef struct {
	unsigned char uUID[8];
	unsigned char uReadCtr[3];
	unsigned char k_SDMFileReadKey[16];
	unsigned char EncryptedTTData[16];
	unsigned char DecryptedTTData[16];
} DECRYPTTAMPERTAGSETTINGS;

typedef struct {
	unsigned char cDiversificationData[32];
	short nDiversificationDataLength;
	unsigned char k_MasterKey[16];
	unsigned char k_DiversifiedKey[16];
}KEYDIVERSIFICATIONDATA;

typedef struct {
	bool uEncryptedFound;
	unsigned char uEncryptedData[16];
	unsigned char uPICCDataTag;
	bool uIDFound;
	unsigned char uID[7];
	bool uReadCtrFound;
	unsigned char uReadCtr[3];
	bool uCMACFound;
	unsigned char uCMACData[8];
	bool uTamperFound;
	unsigned char uTamperTag[16];
}URLPARSEDATA;

extern "C" NTAGLIB_API bool NTAG_Verify_CMAC(unsigned char* k_SDMFileReadKey, unsigned char* uID, short uIDLength, unsigned char* uReadCtr, short uReaderCtrLength, unsigned char* cCMACData, short cCMACDataLen, unsigned char* uCMACValue);
extern "C" NTAGLIB_API void NTAG_CreateCMACSessionKey(unsigned char* k_SDMFileReadKey, unsigned char* uID, short uIDLength, unsigned char* uReadCtr, short uReadCtrLength, unsigned char* k_SesSMDFileReadMAC);
extern "C" NTAGLIB_API void NTAG_AuthenticateEV2First(unsigned char* k_AuthKey, unsigned char* IncomingData, unsigned char* cRndAValue, unsigned char* cRndBValue, unsigned char* OutgoingData);
extern "C" NTAGLIB_API bool NTAG_AuthenticateEV2FirstPart2(unsigned char* k_AuthKey, unsigned char* IncomingData, unsigned char* cRndAValue, unsigned char* cRndBValue, unsigned char* bPDCap2, unsigned char* bPCDCap2, unsigned char* bTI, unsigned char* k_EncryptSessionKey, unsigned char* k_CMACSessionKey);
extern "C" NTAGLIB_API void NTAG_GenerateEncryptData(unsigned char* k_SesAuthEnc, unsigned char* IncomingData, unsigned char* IV, short IncomingLength);
extern "C" NTAGLIB_API void NTAG_GenerateCMAC(unsigned char* k_SesAuthMac, unsigned char* Data, short DataLength, unsigned char* retCMAC);
extern "C" NTAGLIB_API short NTAG_ChangeKey(BYTE nAuthKeyNumber, unsigned char *k_OldKeyValue, BYTE nNewKeyNumber, BYTE nNewKeyVersion, unsigned char * k_NewKeyValue, unsigned char * bTI, unsigned char * uCmdCtr, unsigned char* k_SesAuthEnc, unsigned char* k_SesAuthMac, unsigned char* cKeyData);
extern "C" NTAGLIB_API short NDEF_ChangeFileSettings(unsigned char* k_SesAuthEnc, unsigned char* k_SesAuthMac, unsigned char* bTI, unsigned char* uCmdCtr, BYTE bFileNumber, SDMFILESETTINGS * sFileSettings, unsigned char* cCmdEncData);
extern "C" NTAGLIB_API void NTAG_DecryptData(unsigned char* k_Key, unsigned char* IncomingData, unsigned char* IV, short IncomingLength);
extern "C" NTAGLIB_API void NTAG_DecryptTamperTag(DECRYPTTAMPERTAGSETTINGS * sTamperTagData);
extern "C" NTAGLIB_API void NTAG_DiversifyKey(KEYDIVERSIFICATIONDATA * sDiversifyKey);
extern "C" NTAGLIB_API bool NTAG_Parse_URL(unsigned char* k_SDMMetaReadKey, char* sURL, short nURLLength, URLPARSEDATA * uRetData );
#endif
