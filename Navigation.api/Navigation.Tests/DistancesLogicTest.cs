/**************************************************************************
 *                                                                        *
 *  File:        DistancesLogicTest.cs                                    *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using FluentAssertions;
using Moq;
using Navigation.Business.Logic.Implementations;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using Xunit;

namespace Navigation.Tests
{
    /// <summary>
    /// DistancesLogicTest class.
    /// </summary>
    /// <remarks>
    /// Unit testing is a method of veryfing the correct functionaly of individual source
    /// code units. An unit is the smallest part of an application tested. The purpose of 
    /// testing the units is to isolate each part of a program and to show that the
    /// individual parts are correct. Testing an unit helps to discover problems in the development 
    /// cycle in time.
    /// </remarks>
    public class DistancesLogicTest
    {
        private Mock<IDbService<Distances>> repositoryMock;

        /// <summary>
        /// DistancesLogicTest constructor.
        /// </summary>
        private void SetUpTests()
        {
            /* Constructor is called for every test. */
            repositoryMock = new Mock<IDbService<Distances>>();
        }

        /// <summary>
        /// Do some clean after the test.
        /// Dealocate memory.
        /// </summary>
        private void TearDownTests()
        {
            repositoryMock = null;
        }

        /// <summary>
        /// Test method that will verify if a distance Id exists in the database.
        /// </summary>
        /// <remarks>
        /// The [Fact] attribute is used by the xUnit.net test runner to identify a 'normal' unit test:
        /// a test method that takes no method arguments.
        ///</remarks>
        [Fact]
        public void When_GetDistanceByIdIsCalled_Then_ShouldReturnOkResult()
        {
            /* Steps: */

            /* 1. Arrange the objects, creating and setting them up as necessary. */
            string lookupId = "5cce0fcfd318aa4ad25c64e5";


            SetUpTests();
            repositoryMock
                .Setup(r => r.GetByIdAsync(lookupId))
                .ReturnsAsync(new Distances { Id = lookupId });

            var sut = new DistancesLogic(repositoryMock.Object);

            /* 2. Act on a object. */
            var result = sut.GetByDistanceIdAsync(lookupId);

            /* 3. Assert that something is expected. */
            result.Should().NotBe(true);

            TearDownTests();
        }

        /// <summary>
        /// Test method that will verify DeleteAsync method that deletes a document
        /// based on Id.
        /// </summary>
        [Fact]
        public void When_DeleteCityByIdIsCalled_Then_ShouldReturnOkResult()
        {
            /* Steps: */

            /* 1. Arrange the objects, creating and setting them up as necessary. */
            string lookupId = "5c9f102a441a3e949c3085a5";
            bool flagResponse = true;


            SetUpTests();
            repositoryMock
                .Setup(r => r.DeleteAsync(lookupId))
                .ReturnsAsync(flagResponse);

            var sut = new DistancesLogic(repositoryMock.Object);

            /* 2. Act on a object. */
            var result = sut.DeleteDistanceAsync(lookupId);

            /* 3. Assert that something is expected. */
            result.Should().NotBe(false);

            TearDownTests();
        }

    }
}

