﻿using DateI18N.Models;
using NUnit.Framework;
using System;

namespace DateI18N.Tests.DateWithTimeZoneTests
{
    internal abstract class DateWithTimeZoneTestContext
    {
        protected DateWithTimeZone TestDate;
        protected DateTime DateTimeInfo;
        protected TimeZoneInfo TimeZoneInfo;
        protected Random Rand = new Random();
        protected int DateOffset;

        [SetUp]
        public virtual void SetUp()
        {
            DateOffset = Rand.Next(365);
            DateTimeInfo = DateTime.Now.AddDays(DateOffset);
            TimeZoneInfo = TimeZoneInfo.Local;

            TestDate = new DateWithTimeZone(DateTimeInfo, TimeZoneInfo);
        }
    }

    [TestFixture]
    internal class WhenConstructing : DateWithTimeZoneTestContext
    {
        [Test]
        [ExpectedException(typeof (ArgumentNullException),
            ExpectedMessage = "Value cannot be null.\r\nParameter name: timeZoneInfo")]
        public void ShouldThrowAnExceptionWhenTimeZoneInfoIsNull()
        {
            TestDate = new DateWithTimeZone(DateTimeInfo, null);
        }
    }

    [TestFixture]
    internal class WhenGettingUtcDate : DateWithTimeZoneTestContext
    {
        [Test]
        public void ShouldReturnTheTheCorrectUtcDateFromUserDate()
        {
            var testDate = DateTime.Now.AddDays(DateOffset);
            var utcDifference = (TimeZoneInfo.ConvertTimeToUtc(testDate) - testDate).TotalMinutes;
            var expectedDate = DateTimeInfo.AddMinutes(utcDifference);

            var result = TestDate.GetUtcDateTime();

            Assert.AreEqual(expectedDate, result);
        }

        [Test]
        public void ShouldReturnTheCorrectUtcDateFromUtcDate()
        {
            var testUtcDate = DateTime.UtcNow.AddDays(DateOffset);
            TestDate = new DateWithTimeZone(testUtcDate, TimeZoneInfo);

            var result = TestDate.GetUtcDateTime();

            Assert.AreEqual(testUtcDate, result);
        }
    }

    [TestFixture]
    internal class WhenGettingUserTime : DateWithTimeZoneTestContext
    {
        [Test]
        public void ShouldReturnTheCorrectUserDateFromUserDate()
        {
            var expected = DateTimeInfo;

            var result = TestDate.GetUserDateTime();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldReturnTheCorrectUserDateFromUtcDate()
        {
            var testDate = DateTime.UtcNow.AddDays(DateOffset);
            var utcDifference = (TimeZoneInfo.ConvertTimeFromUtc(testDate, TimeZoneInfo.Local) - testDate).TotalMinutes;
            var expected = testDate.AddMinutes(utcDifference);
            TestDate = new DateWithTimeZone(testDate, TimeZoneInfo);

            var result = TestDate.GetUserDateTime();

            Assert.AreEqual(expected, result);
        }
    }
}