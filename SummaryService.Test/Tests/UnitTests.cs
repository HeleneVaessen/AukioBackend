using NUnit.Framework;
using SummaryService.DAL;
using SummaryService.Models;
using SummaryService.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummaryService.Test.Tests
{
    class UnitTests
    {
        private ISummaryService _summaryService;
        private ISummaryRepository _summaryRepository;

        [SetUp]
        public void SetUp()
        {
            _summaryRepository = new MockSummaryRepository();
            _summaryService = new SummaryService.Services.SummaryService(_summaryRepository);
        }

        [Test]
        public void AddSummaryTest()
        {
            Summary summary = new Summary()
            {
                Content = "Content",
                Id = "",
                SummaryID = Guid.NewGuid(),
                Title = "Title",
                UserId = 2
            };

            _summaryService.PostSummary(summary);

            Assert.AreEqual(_summaryRepository.GetSummaries().Find(e => e.UserId == 2), summary); 
        }

        [Test]
        public void GetSummariesTest()
        {
            Summary summary1 = new Summary()
            {
                Content = "Content",
                Id = "",
                SummaryID = Guid.NewGuid(),
                Title = "Title",
                UserId = 2
            };

            Summary summary2 = new Summary()
            {
                Content = "Content",
                Id = "",
                SummaryID = Guid.NewGuid(),
                Title = "Title",
                UserId = 3
            };

            _summaryService.PostSummary(summary1);
            _summaryService.PostSummary(summary2);

            Assert.AreEqual(_summaryRepository.GetSummaries().Count, 2);
            Assert.AreEqual(_summaryRepository.GetSummaries().Find(e => e.UserId == 2), summary1);
            Assert.AreEqual(_summaryRepository.GetSummaries().Find(e => e.UserId == 3), summary2);
        }
    }
}
