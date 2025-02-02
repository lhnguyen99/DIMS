﻿using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IGenerateQr
    {
        string GenerateQrString(QrInput qri);
        string createQrContent(QrInput qri);

        void GetQrDetail(VertifyQrInput qri , out string bookingID, out string RoomID);
    }
}
