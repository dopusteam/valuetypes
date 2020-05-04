﻿// Copyright 2020 dopusteam - https://www.github.com/dopusteam
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace ValueTypes
{
    /// <summary>
    /// This value type represents a date without time.
    /// Every Date object has a private field (_dateTime) of type DateTime.
    /// Basically Date object wrapped DateTime to provide timeless interface.
    /// </summary>
    [Serializable]
    public struct Date : IEquatable<Date>, IComparable<Date>, ISerializable
    {
        private readonly DateTime _dateTime;

        private const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Returns the year part of this Date.
        /// The returned value is an integer between 1 and 9999.
        /// </summary>
        public int Year => this._dateTime.Year;

        /// <summary>
        /// Returns the month part of this Date.
        /// The returned value is an integer between 1 and 12
        /// </summary>
        public int Month => this._dateTime.Month;

        /// <summary>
        /// Returns the day-of-month part of this Date.
        /// The returned value is an integer between 1 and 31
        /// </summary>
        public int Day => this._dateTime.Day;

        /// <summary>
        /// Returns the day-of-week part of this Date.
        /// The returned value is an integer between 0 and 6,
        /// where 0 indicates Sunday, 1 indicates Monday, 2 indicates Tuesday, 3 indicates Wednesday,
        /// 4 indicates Thursday, 5 indicates Friday, and 6 indicates Saturday
        /// </summary>
        public DayOfWeek DayOfWeek => this._dateTime.DayOfWeek;

        /// <summary>
        /// Returns the day-of-year part of this Date.
        /// The returned value is an integer between 1 and 366.
        /// </summary>
        public int DayOfYear => this._dateTime.DayOfYear;

        #region Private constructors

        private static Date FromDateTime(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException();
            }

            return new Date(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        #endregion

        /// <summary>
        /// Constructs a Date from a given year, month and day
        /// </summary>
        /// <param name="year">Number of year</param>
        /// <param name="month">Number of month</param>
        /// <param name="day">Day of month</param>
        public Date(int year, int month, int day)
        {
            this._dateTime = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }

        public static Date Today
        {
            get
            {
                var date = DateTime.Today;

                return new Date(date.Year, date.Month, date.Day);
            }
        }

        /// <summary>
        /// Convert the specified string representation of a date to its Date equivalent
        /// using the ISO-8601 format (YYYY-MM-DD).
        /// This method returns a value that indicates whether the conversion succeeded
        /// </summary>
        /// <returns>String representation of date in YYYY-MM-DD format</returns>
        public static bool TryParse(string dateString, out Date date)
        {
            var isValidDate = DateTime.TryParseExact(
                dateString,
                Date.DateFormat,
                null,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var dateTime
            );

            if (!isValidDate)
            {
                date = default;

                return false;
            }

            date = Date.FromDateTime(dateTime);

            return true;
        }

        #region Compare dates

        public static bool operator ==(Date firstDate, Date secondDate) => firstDate._dateTime == secondDate._dateTime;

        public static bool operator !=(Date firstDate, Date secondDate) => firstDate._dateTime != secondDate._dateTime;

        public static bool operator <(Date firstDate, Date secondDate) => firstDate._dateTime < secondDate._dateTime;

        public static bool operator <=(Date firstDate, Date secondDate) => firstDate._dateTime <= secondDate._dateTime;

        public static bool operator >(Date firstDate, Date secondDate) => firstDate._dateTime > secondDate._dateTime;

        public static bool operator >=(Date firstDate, Date secondDate) => firstDate._dateTime >= secondDate._dateTime;

        #endregion

        #region Change date

        /// <summary>
        /// Returns the Date resulting from adding a integer number of days to this Date.
        /// The value argument is permitted to be negative.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Date AddDays(int value) => Date.FromDateTime(this._dateTime.AddDays(value));

        /// <summary>
        /// Returns the Date resulting from adding a integer number of months to this Date.
        /// The value argument is permitted to be negative.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Date AddMonths(int value) => Date.FromDateTime(this._dateTime.AddMonths(value));

        /// <summary>
        /// Returns the Date resulting from adding a integer number of years to this Date.
        /// The value argument is permitted to be negative.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Date AddYears(int value) => Date.FromDateTime(this._dateTime.AddYears(value));

        #endregion

        public override int GetHashCode() => this._dateTime.GetHashCode();

        public override bool Equals(object obj)
        {
            return obj is Date other && this.Equals(other);
        }

        public bool Equals(Date other) => this._dateTime.Equals(other._dateTime);

        /// <summary>
        /// Returns the string date representation in YYYY-MM-DD ISO-8601 format
        /// </summary>
        /// <returns>String representation of date in YYYY-MM-DD format</returns>
        public override string ToString() => this._dateTime.ToString(Date.DateFormat);

        /// <summary>
        /// Compares two Date values, returning an integer that indicates their relationship
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Date other) => this._dateTime.CompareTo(other._dateTime);

        public void Deconstruct(out int year, out int month, out int day)
        {
            year = this.Year;
            month = this.Month;
            day = this.Day;
        }

        #region serialization\deserialization

        private Date(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            var dateTime = serializationInfo.GetDateTime(nameof(this._dateTime));

            this._dateTime = dateTime;
        }

        public void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            serializationInfo.AddValue(nameof(Date._dateTime), this._dateTime);
        }

        #endregion
    }
}
