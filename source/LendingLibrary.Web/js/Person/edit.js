var LendingLibrary = LendingLibrary || {};
LendingLibrary.Web = LendingLibrary.Web || {};
LendingLibrary.Web.Person = LendingLibrary.Web.Person || {};
LendingLibrary.Web.Person.Edit = LendingLibrary.Web.Person.Edit || {};

(function (ns) {
    ns.editPersonEntry = function (personId, editUrl, redirectUrl) {
        $.get(editUrl, { id: personId })
            .then(function (response) {
                $.redirect(redirectUrl, response);
            })
            .fail(function (result) {
                sweetAlert({
                    title: "Error: " + result.statusText,
                    text: "",
                    type: "error",
                    confirmButtonColor: "#7CB43F"
                });
            });
    };
})(LendingLibrary.Web.Person.Edit);