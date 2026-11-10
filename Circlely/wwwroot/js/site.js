document.addEventListener("DOMContentLoaded", function () {

document.querySelectorAll(".circle-comment-box").forEach(form => {

    form.addEventListener("submit", async function (event) {

        event.preventDefault();

        const input = form.querySelector(".circle-comment-input");
        const button = form.querySelector(".circle-comment-submit");

        const content = input.value.trim();
        const postId = form.querySelector('[name="postId"]').value;
        const token = form.querySelector(
            'input[name="__RequestVerificationToken"]'
        ).value;

        if (!content) {
            alert("Please enter a comment.");
            return;
        }

        button.disabled = true;

        try {

            const response = await fetch(form.dataset.url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "RequestVerificationToken": token
                },
                body: new URLSearchParams({
                    postId: postId,
                    content: content,
                    __RequestVerificationToken: token
                })
            });

            const result = await response.json();

            if (!response.ok || !result.success) {
                throw new Error(result.message || "Unable to add comment.");
            }

            const commentsList = form.parentElement.querySelector(
                ".circle-comments-list"
            );

            if (commentsList) {

                const commentElement = document.createElement("div");
                commentElement.className = "circle-comment-item";

                const username = document.createElement("strong");
                username.className = "circle-comment-username";
                username.textContent = result.comment.userName;

                const commentText = document.createElement("span");
                commentText.className = "circle-comment-content";
                commentText.textContent = result.comment.content;

                commentElement.appendChild(username);
                commentElement.appendChild(commentText);

                commentsList.appendChild(commentElement);
            }

            input.value = "";

            const countButton = form.parentElement.querySelector(
                ".circle-view-comments"
            );

            if (countButton) {
                countButton.textContent =
                    `View all ${ result.commentCount } comments`;
            }

            console.log("Comment added successfully!");

        } catch (error) {

            console.error("Comment error:", error);

            alert(error.message);

        } finally {

            button.disabled = false;

        }

    });

});


});
