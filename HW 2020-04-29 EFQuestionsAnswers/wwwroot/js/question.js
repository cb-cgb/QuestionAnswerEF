$(() => {

    //like a question
    $("#btn-like-quest").on('click',function () {

        const questionId = $(this).data('qid');
        const userId = $(this).data('uid');

        //add a like
        $.post("/home/AddQuestionLike", { questionId }, function () {

            //Update the likes count
            updateQuestionLikes();

            //disable the like button once the user liked it once already.
            $.get('/home/GetQuestionLikesForCurrentUser', { questionId, userId }, result => {
                if (result > 0) {
                    $("#btn-like-quest").prop('disabled', true);
                };
            });
        });
    }); 


    //like an answer
    $("#btn-like-answ").on('click', function () {
        const answerId = $(this).data('aid');

      //add a like
        $.post("/home/AddAnswerLike", { answerId }, function () {
            //Update the likes count
            updateAnswerLikes();

            //disable the like buttong onece the user liked it
            $("#btn-like-answ").prop('disabled', true);
        })

    });


    //run updateLikes every 2 seconds
    setInterval(updateLikes, 2000);

    //update both likes
    function updateLikes()     {
        updateQuestionLikes();
        updateAnswerLikes();
    }


    //update count of questionlikes
    function updateQuestionLikes() {
        const questionId = $("#likes-quest-count").data('qid');

        $.get('/home/GetQuestionLikes', { questionId }, result => {
            $("#likes-quest-count").text(result);
            console.log(`question countlike: ${result}`);
        })
    }

    //update count of answerlikes
    function updateAnswerLikes() {
        const answerId = $("#likes-answ-count").data('aid');

        $.get('/home/GetAnswerLikes', { answerId }, result => {
            $("#likes-answ-count").text(result);
            console.log(`answer likecount ${result}`);
        })

    }
    
       
    

   
    //const updateLikes = () => {
    //    const questionId = $("#likes-quest-count").data('qid');
    //    $.get('/home/GetQuestionLikes', { questionId }, result => {
    //        //$("#likes-quest-count").text(result.countLikes); //worked
    //        $("#likes-quest-count").text(result); //worked
    //        console.log(result);       
    //        console.log(result.countlikes);            
    //    });
    //};

   


   

});