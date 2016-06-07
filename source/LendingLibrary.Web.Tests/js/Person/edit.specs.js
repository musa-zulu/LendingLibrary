describe('person/delete.js', function () {
    it('should create the LendingLibrary.Web.Person.Index',
        function () {
            expect(LendingLibrary.Web.Person.Delete).toBeDefined();
        });
    var toRemove = [];
    var mkContext = function () {
        var ctx = $('<div></div>');
        toRemove.push(ctx);
        $('body').append(ctx);
        return ctx;
    };
    var createInput = function (id, value) {
        var input = $('<input></input>');
        input.attr('id', id);
        input.attr('value', value);
        return input;
    };
    afterEach(function () {
        toRemove.forEach(function (item) {
            item.remove();
        });
    });
    describe('editPersonEntry', function () {
        it('should create the LendingLibrary.Web.Person.Index.editPersonEntry', function () {
            expect(LendingLibrary.Web.Person.Edit.editPersonEntry).toBeDefined();
        });

        it('should do a get call given a valid url and personId', function () {
            // arrange 
            var personId = 3;
            var editUrl = "/foo/bar";
            var redirectUrl = "/foo/bar";
            var deferred = $.Deferred();
            spyOn($, "get").and.returnValue(deferred);
            var ctx = mkContext();
            // act
            LendingLibrary.Web.Person.Edit.editPersonEntry(personId, editUrl, redirectUrl);
            // assert   
            expect($.get).toHaveBeenCalledWith(editUrl, { id: personId });
        });
    });
});