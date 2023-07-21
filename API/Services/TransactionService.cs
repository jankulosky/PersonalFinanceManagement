﻿using API.Data.Interfaces;
using API.DTOs;
using API.Helpers;
using API.Models;
using API.Services.Interfaces;

namespace API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionDto> CategorizeTransactionAsync(int id, CategorizeTransactionDto catCode)
        {
            return await _transactionRepository.CategorizeSingleTransaction(id, catCode);
        }

        public async Task<PagedList<TransactionDto>> GetListAsync(FileParams fileParams)
        {
            return await _transactionRepository.GetTransactionList(fileParams);
        }

        public async Task<List<AnalyticsDto>> GetTransactionAnalyticsAsync(AnalyticsParams analyticsParams)
        {
            return await _transactionRepository.GetTransactionAnalytics(analyticsParams);
        }

        public async Task<List<Transaction>> ImportTransactionsAsync(IFormFile csv)
        {
            return await _transactionRepository.ImportTransactionsFromFile(csv);
        }
    }
}
