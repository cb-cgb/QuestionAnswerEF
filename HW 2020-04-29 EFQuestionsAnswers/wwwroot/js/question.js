$(() => {

    $("#btn-like-quest").on('click',function () {

        const questionId = $(this).data('qid');
        const userId = $(this).data('uid');

        //add a like
        $.post("/home/AddQuestionLike", { questionId }, function () {

            //Update the likes count
            updateLikes();

            //disable the like button once the user liked it once already.
            $.get('/home/GetQuestionLikesForCurrentUser', { questionId, userId }, result => {
                if (result > 0) {
                    $("#btn-like-quest").prop('disabled', true);
                };
            });
        });
        

    }); 

    function updateLikes() {
        const questionId = $("#likes-quest-count").data('qid');
        $.get('/home/GetQuestionLikes',{questionId}, result => {
            $("#likes-quest-count").text (result);
            console.log(result);            
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

   
    setInterval(updateLikes, 2000);

    //$("#answers").on('click','.answer', function () {

    //    const la = {
    //        answerId: $(this).data('aid'),
    //        userId: $(this).data('uid'),
    //        questionid: $(this).data('qid')
    //    };

    //    $.post("/home/AddAnswerLike", la, function () {
    //        $("#likes-answ-count").val = obj.countLikes;
    //        if (!obj.countLikes === null && obj.countikes !== 0) {
    //            $(this).prop('disabled', true);
    //        }
            
    //    })

    //})
   

});