var LendingLibrary = LendingLibrary || {};
LendingLibrary.Web = LendingLibrary.Web || {};
LendingLibrary.Web.Person = LendingLibrary.Web.Person || {};
LendingLibrary.Web.Person.Delete = LendingLibrary.Web.Person.Delete || {};

(function (ns) {
    ns.deletePerson = function (personId, deleteUrl, redirectUrl) {
        sweetAlert(
                 {
                     title: "Are you sure you would like to delete this Person?",
                     type: "warning",
                     showCancelButton: true,
                     confirmButtonColor: "#DD6B55",
                     confirmButtonText: "Confirm",
                     cancelButtonText: "Cancel",
                     closeOnConfirm: false,
                     closeOnCancel: true
                 },
                 function (isConfirm) {
                     if (isConfirm) {
                         $.post(deleteUrl, { id: personId }).then(function () {
                             sweetAlert({
                                 title: "Person deleted successfully",
                                 text: "",
                                 type: "success",
                                 confirmButtonColor: "#7CB43F"
                             },
                                 function () {
                                     $.redirect(redirectUrl);
                                 });
                         }).fail(function (result) {
                             sweetAlert({
                                 title: "Error: " + result.statusText,
                                 text: "",
                                 type: "error",
                                 confirmButtonColor: "#7CB43F"
                             });
                         });
                     }
                 });
    };
})(LendingLibrary.Web.Person.Delete);