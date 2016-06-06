var LendingLibrary = LendingLibrary || {};
LendingLibrary.Web = LendingLibrary.Web || {};
LendingLibrary.Web.Lending = LendingLibrary.Web.Lending || {};
LendingLibrary.Web.Lending.Delete = LendingLibrary.Web.Lending.Delete || {};
(function(ns) {
    ns.deleteLendingEntry = function (lendingId, deleteUrl, redirectUrl) {
        sweetAlert(
           {
               title: "Are you sure you would like to delete this lending entry?",
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
                   $.post(deleteUrl, { newlendingId: newlendingId }).then(function (response) {
                       sweetAlert({
                           title: "Lending entry deleted successfully",
                           text: "",
                           type: "success",
                           confirmButtonColor: "#7CB43F"
                       },
                           function () {
                               $.redirect(redirectUrl, { lendingId: lendingId });
                           });
                   }).fail(function (result) {
                       alert(isConfirm + " " + lendingId);
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
})(LendingLibrary.Web.Lending.Delete);