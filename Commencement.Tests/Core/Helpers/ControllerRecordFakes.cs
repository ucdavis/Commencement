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
        /// <summary>
        /// Fakes the student.
        /// Note: Using more than 20 will not be reliable because the Guids will be random
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="studentRepository">The student repository.</param>
        public static void FakeStudent(int count, IRepositoryWithTypedId<Student, Guid> studentRepository)
        {
            var students = new List<Student>();
            FakeStudent(count, studentRepository, students);
        }

        /// <summary>
        /// Fakes the student.
        /// Note: Using more than 20 will not be reliable because the Guids will be random
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="studentRepository">The student repository.</param>
        /// <param name="specificStudents">The specific students.</param>
        public static void FakeStudent(int count, IRepositoryWithTypedId<Student, Guid> studentRepository, List<Student> specificStudents)
        {
            var students = new List<Student>();
            var specificStudentsCount = 0;
            if (specificStudents != null)
            {
                specificStudentsCount = specificStudents.Count;
                for (int i = 0; i < specificStudentsCount; i++)
                {
                    students.Add(specificStudents[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                students.Add(CreateValidEntities.Student(i + specificStudentsCount + 1));
            }

            var totalCount = students.Count;
            for (int i = 0; i < totalCount; i++)
            {
                students[i].SetIdTo(SpecificGuid.GetGuid(i+1));
                int i1 = i;
                studentRepository
                    .Expect(a => a.GetNullableById(students[i1+1].Id))
                    .Return(students[i])
                    .Repeat
                    .Any();
            }
            studentRepository.Expect(a => a.GetNullableById(SpecificGuid.GetGuid(totalCount + 1))).Return(null).Repeat.Any();
            studentRepository.Expect(a => a.Queryable).Return(students.AsQueryable()).Repeat.Any();
            studentRepository.Expect(a => a.GetAll()).Return(students).Repeat.Any();
        }

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
