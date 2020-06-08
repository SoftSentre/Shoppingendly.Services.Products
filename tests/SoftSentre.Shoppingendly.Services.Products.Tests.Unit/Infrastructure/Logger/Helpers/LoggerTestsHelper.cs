// Copyright 2020 SoftSentre Contributors
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
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Logger.Helpers
{
    public static class LoggerTestHelper
    {
        public static ISetup<ILogger<T>> Setup<T>(this Mock<ILogger<T>> logger, LogLevel level)
        {
            return logger.Setup(x => x.Log(level, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(), It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        public static ISetup<ILogger<T>> Setup<T>(this Mock<ILogger<T>> logger, LogLevel level, string message)
        {
            return logger.Setup(x => x.Log(level, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Equals(message, StringComparison.InvariantCultureIgnoreCase)), It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
        }

        public static ISetup<ILogger<T>> Setup<T>(this Mock<ILogger<T>> logger, LogLevel level, string message,
            Exception exception)
        {
            return logger.Setup(x => x.Log(level, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Equals(message, StringComparison.InvariantCultureIgnoreCase)), exception,
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        public static void Verify<T>(this Mock<ILogger<T>> mock, LogLevel level, Times times)
        {
            mock.Verify(Verify<T>(level), times);
        }

        public static void Verify<T>(this Mock<ILogger<T>> mock, LogLevel level, string message, Times times)
        {
            mock.Verify(Verify<T>(level, message), times);
        }

        public static void Verify<T>(this Mock<ILogger<T>> mock, LogLevel level, string message, Exception exception,
            Times times)
        {
            mock.Verify(Verify<T>(level, message, exception), times);
        }

        private static Expression<Action<ILogger<T>>> Verify<T>(LogLevel level)
        {
            return x => x.Log(level, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true));
        }

        private static Expression<Action<ILogger<T>>> Verify<T>(LogLevel level, string message)
        {
            return x => x.Log(level, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Equals(message, StringComparison.InvariantCultureIgnoreCase)), It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true));
        }

        private static Expression<Action<ILogger<T>>> Verify<T>(LogLevel level, string message, Exception exception)
        {
            return x => x.Log(level, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Equals(message, StringComparison.InvariantCultureIgnoreCase)), exception,
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true));
        }
    }
}