using SME.Models;

namespace SME.Services{
    public interface IDatabaseCommunicator
    {
        QuestionBatchResponse ProvideQuestionsFromId(QuestionBatchRequest batchRequest);
        void HandleQuestionRequestFromQueue();
    }
}