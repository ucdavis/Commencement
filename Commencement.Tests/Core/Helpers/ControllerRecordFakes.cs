using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;

namespace Commencement.Tests.Core.Helpers
{
    /// <summary>
    /// Static methods to fake collections of records for controller tests.
    /// </summary>
    public static class ControllerRecordFakes
    {
        //Example...

        ///// <summary>
        ///// Fakes the transaction.
        ///// </summary>
        ///// <param name="count">The count.</param>
        ///// <param name="transactionRepository">The transaction repository.</param>
        //public static void FakeTransaction(int count, IRepository<Transaction> transactionRepository)
        //{
        //    var transactions = new List<Transaction>();
        //    FakeTransaction(count, transactionRepository, transactions);
        //}

        ///// <summary>
        ///// Fakes the transaction.
        ///// </summary>
        ///// <param name="count">The count.</param>
        ///// <param name="transactionRepository">The transaction repository.</param>
        ///// <param name="specificTransactions">The specific transactions.</param>
        //public static void FakeTransaction(int count, IRepository<Transaction> transactionRepository, List<Transaction> specificTransactions)
        //{
        //    var transactions = new List<Transaction>();
        //    var specificTransactionsCount = 0;
        //    if (specificTransactions != null)
        //    {
        //        specificTransactionsCount = specificTransactions.Count;
        //        for (int i = 0; i < specificTransactionsCount; i++)
        //        {
        //            transactions.Add(specificTransactions[i]);
        //        }
        //    }

        //    for (int i = 0; i < count; i++)
        //    {
        //        transactions.Add(CreateValidEntities.Transaction(i + specificTransactionsCount + 1));
        //    }

        //    var totalCount = transactions.Count;
        //    for (int i = 0; i < totalCount; i++)
        //    {
        //        transactions[i].SetIdTo(i + 1);
        //        int i1 = i;
        //        transactionRepository
        //            .Expect(a => a.GetNullableById(i1 + 1))
        //            .Return(transactions[i])
        //            .Repeat
        //            .Any();
        //    }
        //    transactionRepository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
        //    transactionRepository.Expect(a => a.Queryable).Return(transactions.AsQueryable()).Repeat.Any();
        //    transactionRepository.Expect(a => a.GetAll()).Return(transactions).Repeat.Any();
        //}
    }
}
