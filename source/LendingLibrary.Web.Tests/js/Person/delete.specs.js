describe('person/delete.js', function () {
    it('should create the LendingLibrary.Web.Person.Index',
        function () {
            expect(LendingLibrary.Web.Person.Delete).toBeDefined();
        });
    var toRemove = [];
    afterEach(function () {
        toRemove.forEach(function (item) {
            item.remove();
        });
    });
    describe('deleteLendingEntry', function () {
        it('should create the LendingLibrary.Web.Person.Index.deletePerson', function () {
            expect(LendingLibrary.Web.Person.Delete.deletePerson).toBeDefined();
        });
        it('should should alert the user', function () {
            // arrange
            var personId = 1;
            var deleteUrl = '/foo/bar';
            var redirectUrl = '/bar/the/foo';
            spyOn(window, 'sweetAlert');
            // act
            LendingLibrary.Web.Person.Delete.deletePerson(personId, deleteUrl, redirectUrl);
            // assert
            expect(window.sweetAlert).toHaveBeenCalled();
        });
    });
});