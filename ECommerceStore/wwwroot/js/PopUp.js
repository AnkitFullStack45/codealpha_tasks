document.addEventListener("DOMContentLoaded", function () {

        setTimeout(function () {

            var loginPopup = new bootstrap.Modal(
                document.getElementById("loginPopup")
            );

            loginPopup.show();

        }, 3000);

    });
