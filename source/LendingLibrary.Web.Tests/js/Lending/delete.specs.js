describe('lending/delete.js', function () {
    it('should create the LendingLibrary.Web.Lending.Index',
        function () {
            expect(LendingLibrary.Web.Lending.Delete).toBeDefined();
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
    describe('deleteLendingEntry', function () {
        it('should create the LendingLibrary.Web.Lending.Index.deleteLendingEntry', function () {
            expect(LendingLibrary.Web.Lending.Delete.deleteLendingEntry).toBeDefined();
        });
        it('should should alert the user', function () {
            // arrange
            var lendingId = 1;
            var deleteUrl = '/foo/bar';
            var redirectUrl = '/bar/the/foo';
            spyOn(window, 'sweetAlert');
            // act
            LendingLibrary.Web.Lending.Delete.deleteLendingEntry(lendingId, deleteUrl, redirectUrl);
            // assert
            expect(window.sweetAlert).toHaveBeenCalled();
        });
        //it('should call $.redirect with given url and parameters given a successful call to $.post', function () {
        //    // arrange
        //    var lendingId = 1;
        //    var deleteUrl = '/foo/bar';
        //    var redirectUrl = '/bar/the/foo';
        //    var deferred = $.Deferred();
        //    spyOn($, 'post').and.returnValue(deferred);
        //    spyOn($, 'redirect');
        //    // act
        //    LendingLibrary.Web.Lending.Delete.deleteLendingEntry(lendingId, deleteUrl, redirectUrl);
        //    deferred.resolve();
        //    // assert
        //    expect($.redirect).toHaveBeenCalledWith(redirectUrl, { redirectUrl });
        //});
    });
});