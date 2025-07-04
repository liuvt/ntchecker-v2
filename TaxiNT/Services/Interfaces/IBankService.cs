﻿using TaxiNT.Libraries.Models.GGSheets;

namespace TaxiNT.Services.Interfaces;
public interface IBankService
{
    Task<Bank> GetBank(string bankId);
    Task<Bank> GetBank(string _SpreadSheetId, string _sheetBANK, string bankId);
}
