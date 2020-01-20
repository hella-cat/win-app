﻿/*
 * Copyright (c) 2020 Proton Technologies AG
 *
 * This file is part of ProtonVPN.
 *
 * ProtonVPN is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * ProtonVPN is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with ProtonVPN.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.ServiceModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ProtonVPN.Common.Extensions;

namespace ProtonVPN.Common.Test.Extensions
{
    [TestClass]
    public class ExceptionTypeExtensionsTest
    {
        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void IsExpectedExceptionOf_ShouldBe_Origin_IsExpectedException(bool expected)
        {
            // Arrange
            var exception = new Exception();
            var origin = Substitute.For<IThrowsExpectedExceptions>();
            origin.IsExpectedException(exception).Returns(expected);

            // Act
            var result = exception.IsExpectedExceptionOf(origin);

            // Assert
            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(true, typeof(IOException))]
        [DataRow(true, typeof(UnauthorizedAccessException))]
        [DataRow(false, typeof(ArgumentException))]
        [DataRow(false, typeof(Exception))]
        public void IsFileAccessException_ShouldBe(bool expected, Type exceptionType)
        {
            // Arrange
            var exception = (Exception)Activator.CreateInstance(exceptionType);

            // Act
            var result = exception.IsFileAccessException();

            // Assert
            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(true, typeof(CommunicationException))]
        [DataRow(false, typeof(IOException))]
        [DataRow(false, typeof(Exception))]
        public void IsCommunicationException_ShouldBe(bool expected, Type exceptionType)
        {
            // Arrange
            var exception = (Exception)Activator.CreateInstance(exceptionType);

            // Act
            var result = exception.IsCommunicationException();

            // Assert
            result.Should().Be(expected);
        }

        [TestMethod]
        public void IsCommunicationException_ShouldBeFalse_When_ObjectDisposedException()
        {
            // Arrange
            var exception = new ObjectDisposedException("System.String");

            // Act
            var result = exception.IsCommunicationException();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsCommunicationException_ShouldBeTrue_WhenSpecific_ObjectDisposedException()
        {
            // Arrange
            var exception = new ObjectDisposedException("System.ServiceModel.Channels.ServiceChannel");

            // Act
            var result = exception.IsCommunicationException();

            // Assert
            result.Should().BeTrue();
        }
    }
}
