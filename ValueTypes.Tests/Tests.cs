using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace ValueTypes.Tests
{
    public class Tests
    {
        [Fact]
        public void ParsingTests()
        {
            var validDate = "2010-01-01";

            Assert.True(Date.TryParse(validDate, out var _));

            var firstInvalidDate = "22002200";
            var secondInvalidDate = "01-01-2010";

            Assert.False(Date.TryParse(firstInvalidDate, out var _));
            Assert.False(Date.TryParse(secondInvalidDate, out var _));
        }

        [Fact]
        public void ToStringTests()
        {
            var date = new Date(2001, 1, 1);

            var isoDate = date.ToString();

            Assert.Equal("2001-01-01", isoDate);
        }

        [Fact]
        public void ObjectEqualsTests()
        {
            var date = new Date(2001, 1, 1);
            var sameDate = new Date(2001, 1, 1);
            var otherDate = new Date(2002, 1, 1);

            Assert.True(date.Equals(sameDate));
            Assert.True(date.Equals((object) sameDate));
            Assert.False(date.Equals(otherDate));
            Assert.False(date.Equals((object) otherDate));
        }

        [Fact]
        public void StringCompatibilityTests()
        {
            var date = new Date(2001, 1, 1);

            var dateString = date.ToString();

            var dateStringIsValid = Date.TryParse(dateString, out var dateFromString);

            Assert.True(dateStringIsValid);
            Assert.Equal(date, dateFromString);
        }

        [Fact]
        public void ChangeDateTests()
        {
            var date = new Date(2010, 1, 1);

            var previousYearDate = date.AddYears(-1);

            Assert.Equal(2009, previousYearDate.Year);
            Assert.Equal(1, previousYearDate.Month);
            Assert.Equal(1, previousYearDate.Day);
        }

        [Fact]
        public void HashTests()
        {
            var firstDate = new Date(2010, 1, 1);
            var secondDate = new Date(2011, 1, 1);

            var firstHash = firstDate.GetHashCode();
            var secondHash = secondDate.GetHashCode();

            Assert.NotEqual(firstHash, secondHash);

            var secondDatePreviousYear = secondDate.AddYears(-1);

            secondHash = secondDatePreviousYear.GetHashCode();

            Assert.Equal(firstHash, secondHash);
        }

        [Fact]
        public void EqualsTests()
        {
            var firstDate = new Date(2010, 1, 1);
            var secondDate = new Date(2011, 1, 1);

            Assert.False(firstDate.Equals(secondDate));

            var secondDatePreviousYear = secondDate.AddYears(-1);

            Assert.True(firstDate.Equals(secondDatePreviousYear));
        }

        [Fact]
        public void TodayTests()
        {
            var today = Date.Today;

            var date = DateTime.Now;

            Assert.Equal(today.Year, date.Year);
            Assert.Equal(today.Month, date.Month);
            Assert.Equal(today.Day, date.Day);
        }

        [Fact]
        public void DictionaryTests()
        {
            var dictionary = new Dictionary<Date, bool>();

            var firstDate = new Date(2010, 1, 1);
            var secondDate = new Date(2011, 1, 1);
            var duplicatedFirstDate = new Date(2010, 1, 1);

            dictionary.Add(firstDate, true);

            Assert.True(dictionary.ContainsKey(firstDate));
            Assert.True(dictionary.ContainsKey(duplicatedFirstDate));

            Assert.True(dictionary[firstDate]);
            Assert.True(dictionary[duplicatedFirstDate]);
            Assert.False(dictionary.ContainsKey(secondDate));

            dictionary.Add(secondDate, false);

            Assert.True(dictionary.ContainsKey(secondDate));
            Assert.False(dictionary[secondDate]);
        }

        [Fact]
        public void HashSetTests()
        {
            var hashSet = new HashSet<Date>();

            var firstDate = new Date(2010, 1, 1);
            var secondDate = new Date(2011, 1, 1);

            hashSet.Add(firstDate);

            Assert.Contains(firstDate, hashSet);
            Assert.DoesNotContain(secondDate, hashSet);

            hashSet.Add(secondDate);

            Assert.Contains(firstDate, hashSet);
            Assert.Contains(secondDate, hashSet);
        }

        [Fact]
        public void SerializationTests()
        {
            var data = new Date(2001, 1, 1);

            var memoryStream = new MemoryStream();

            var binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(memoryStream, data);

            memoryStream.Position = 0;

            var deserializedDate = binaryFormatter.Deserialize(memoryStream);

            Assert.Equal(data, deserializedDate);
        }

        [Fact]
        public void ComparisonTests()
        {
            var firstDate = new Date(2001, 1, 1);
            var secondDate = new Date(2001, 1, 1);
            var thirdDate = new Date(2002, 1, 1);

            Assert.False(firstDate < secondDate);
            Assert.False(firstDate > secondDate);
            Assert.False(firstDate != secondDate);
            Assert.True(firstDate >= secondDate);
            Assert.True(firstDate <= secondDate);
            Assert.True(firstDate == secondDate);

            Assert.False(secondDate > firstDate);
            Assert.False(secondDate < firstDate);
            Assert.True(secondDate <= firstDate);
            Assert.True(secondDate >= firstDate);

            Assert.True(firstDate < thirdDate);
            Assert.True(firstDate <= thirdDate);
            Assert.True(firstDate != thirdDate);

            Assert.False(firstDate > thirdDate);
            Assert.False(firstDate >= thirdDate);
            Assert.False(firstDate == thirdDate);

            Assert.True(secondDate < thirdDate);
            Assert.True(secondDate <= thirdDate);
            Assert.True(secondDate != thirdDate);

            Assert.False(secondDate > thirdDate);
            Assert.False(secondDate >= thirdDate);
            Assert.False(secondDate == thirdDate);
        }

        [Fact]
        public void SortTests()
        {
            var firstDate = new Date(2001, 1, 1);
            var secondDate = new Date(2010, 1, 1);
            var thirdDate = new Date(2005, 1, 1);

            var dates = new List<Date>
            {
                firstDate,
                secondDate,
                thirdDate
            };

            Assert.Equal(dates[0], firstDate);
            Assert.Equal(dates[1], secondDate);
            Assert.Equal(dates[2], thirdDate);

            dates.Sort();

            Assert.Equal(dates[0], firstDate);
            Assert.Equal(dates[1], thirdDate);
            Assert.Equal(dates[2], secondDate);
        }

        [Fact]
        public void AddAndSubtractTests()
        {
            var date = new Date(2001, 1, 1);

            var nextDay = date.AddDays(1);

            Assert.Equal(2001, nextDay.Year);
            Assert.Equal(1, nextDay.Month);
            Assert.Equal(2, nextDay.Day);

            var nextMonth = nextDay.AddMonths(1);

            Assert.Equal(2001, nextMonth.Year);
            Assert.Equal(2, nextMonth.Month);
            Assert.Equal(2, nextMonth.Day);

            var nextYear = nextMonth.AddYears(1);

            Assert.Equal(2002, nextYear.Year);
            Assert.Equal(2, nextYear.Month);
            Assert.Equal(2, nextYear.Day);

            var previousMonth = nextYear.AddMonths(-2);

            Assert.Equal(2001, previousMonth.Year);
            Assert.Equal(12, previousMonth.Month);
            Assert.Equal(2, previousMonth.Day);

            var previousDay = previousMonth.AddDays(-2);

            Assert.Equal(2001, previousDay.Year);
            Assert.Equal(11, previousDay.Month);
            Assert.Equal(30, previousDay.Day);
        }

        [Fact]
        public void DayOfWeekTests()
        {
            var wednesday = new Date(2020, 1, 1);

            Assert.Equal(DayOfWeek.Wednesday, wednesday.DayOfWeek);

            var thursday = wednesday.AddDays(1);

            Assert.Equal(DayOfWeek.Thursday, thursday.DayOfWeek);

            var monday = thursday.AddDays(-3);

            Assert.Equal(DayOfWeek.Monday, monday.DayOfWeek);
        }

        [Fact]
        public void DayOfYearTests()
        {
            var day1 = new Date(2010, 1, 1);

            Assert.Equal(1, day1.DayOfYear);

            var day123 = day1.AddDays(122);

            Assert.Equal(123, day123.DayOfYear);

            var day12 = day123.AddDays(-111);

            Assert.Equal(12, day12.DayOfYear);
        }

        [Fact]
        public void DeconstructTests()
        {
            var date = new Date(2001, 1, 31);

            var (year, month, day) = date;

            Assert.Equal(2001, year);
            Assert.Equal(1, month);
            Assert.Equal(31, day);
        }
    }
}
