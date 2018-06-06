import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';
import { stat } from 'fs';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.
export interface EntryHeader {
  id: number;
  title: string;
}

export interface Entry extends EntryHeader {
  text: string;
  link: string;
  updateTime: Date;
  topicId: number;
}

export interface TopicHeader {
  id: number;
  name: string;
}

export interface Topic extends TopicHeader {
  updateTime: Date;
  ownerId: number;
  entryHeaders: EntryHeader[];
}

export interface LearningLogsState {
  isLoading: boolean;
  message: string;
  ownerId: number;
  topics: TopicHeader[];
  selectedTopic?: Topic;
  topicId: number;
  selectedEntry?: Entry;
  entryId: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTopicsAction {
  type: 'REQUEST_LEARNING_LOG_TOPICS';
  ownerId: number;
}

interface ReceiveTopicsAction {
  type: 'RECEIVE_LEARNING_LOG_TOPICS';
  ownerId: number;
  topics: TopicHeader[];
  message: string;
}

interface SelectTopicAction {
  type: 'SELECT_LEARNING_LOG_TOPIC';
  topicId: number;
}

interface RequestTopicDetailAction {
  type: 'REQUEST_TOPIC_DETAIL';
  topicId: number;
}

interface ReciveTopicDetailAction {
  type: 'RECEIVE_TOPIC_DETAIL';
  topic: Topic;
  message: string;
}

interface SelectEntryAction {
  type: 'SELECT_TOPIC_ENTRY';
  entryId: number;
}

interface RequestEntryDetailAction {
  type: 'REQUEST_ENTRIY_DETAIL';
  entryId: number;
}

interface ReciveEntryDetailAction {
  type: 'RECEIVE_ENTRIE_DETAIL';
  entry: Entry;
  message: string;
}

interface PostEntryDetailAction {
  type: 'POST_ENTRY_DETAIL';
  entrydetail: Entry;
}

interface RecivePostEntryDetailAction {
  type: 'RECEIVE_POST_ENTRIE_DETAIL';
  message: string;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction =
  | RequestTopicsAction
  | ReceiveTopicsAction
  | SelectTopicAction
  | RequestTopicDetailAction
  | ReciveTopicDetailAction
  | SelectEntryAction
  | RequestEntryDetailAction
  | ReciveEntryDetailAction
  | PostEntryDetailAction
  | RecivePostEntryDetailAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
  //when mount compoment, or user changed
  requestTopics: (ownerId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    if (ownerId !== getState().learningLogs.ownerId) {
      let fetchTask = fetch(`api/SampleData/UserTopics?userId=${ownerId}`)
        .then(response => response.json() as Promise<TopicHeader[]>)
        .then(data => {
          dispatch({
            type: 'RECEIVE_LEARNING_LOG_TOPICS',
            topics: data,
            ownerId,
            message: ''
          });
        })
        .catch(err => {
          dispatch({
            type: 'RECEIVE_LEARNING_LOG_TOPICS',
            topics: [],
            ownerId,
            message: err.message
          });
        });
      addTask(fetchTask);
      dispatch({ type: 'REQUEST_LEARNING_LOG_TOPICS', ownerId });
    }
  },
  //when click on topic header, load topic's entries or collaps topic
  selectTopic: (topicId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    if (topicId !== getState().learningLogs.topicId) {
      let fetchTask = fetch(`api/SampleData/TopicDetail?topicId=${topicId}`)
        .then(response => response.json() as Promise<Topic>)
        .then(data => {
          dispatch({
            type: 'RECEIVE_TOPIC_DETAIL',
            topic: data,
            message: ''
          });
        })
        .catch(err => console.log(err.message));
      addTask(fetchTask);
      dispatch({ type: 'REQUEST_TOPIC_DETAIL', topicId });
    } else {
      dispatch({ type: 'SELECT_LEARNING_LOG_TOPIC', topicId });
    }
  },
  selectEntry: (entryId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    if (entryId !== getState().learningLogs.entryId) {
      let fetchTask = fetch(`api/SampleData/EntryDetail?entryId=${entryId}`)
        .then(response => response.json() as Promise<Entry>)
        .then(data => {
          dispatch({
            type: 'RECEIVE_ENTRIE_DETAIL',
            entry: data,
            message: ''
          });
        })
        .catch(err => console.log(err.message));
      addTask(fetchTask);
      dispatch({ type: 'REQUEST_ENTRIY_DETAIL', entryId });
    } else {
      dispatch({ type: 'SELECT_TOPIC_ENTRY', entryId });
    }
  }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: LearningLogsState = {
  ownerId: -1,
  isLoading: false,
  message: '',
  topics: [],
  topicId: -1,
  entryId: -1
};

export const reducer: Reducer<LearningLogsState> = (
  state: LearningLogsState,
  incomingAction: Action
) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case 'REQUEST_LEARNING_LOG_TOPICS':
      return {
        isLoading: true,
        message: 'loading...',
        ownerId: action.ownerId,

        //loading new owner info, clear old data
        topics: [],
        topicId: -1,
        entryId: -1
        //selectedTopic: state.selectedTopic,
        //selectedEntry: state.selectedEntry,
      };
    case 'RECEIVE_LEARNING_LOG_TOPICS':
      // Only accept the incoming data if it matches the most recent request. This ensures we correctly
      // handle out-of-order responses.
      if (action.ownerId === state.ownerId) {
        return {
          isLoading: false,
          message: action.message,
          topics: action.topics,

          ownerId: state.ownerId,
          topicId: -1,
          entryId: -1
          //when recived new topics, selected topic and entry still unset
          //selectedtopic: state.selectedtopic,
          //selectedentry: state.selectedentry,
        };
      }
      break;
    case 'SELECT_LEARNING_LOG_TOPIC':
      //wait for recive_topic_detail to update, or just hide
      return {
        //if click topic twice, collaps topic
        topicId: state.topicId === action.topicId ? -1 : action.topicId,

        ownerId: state.ownerId,
        isLoading: state.isLoading,
        message: state.message,
        topics: state.topics,

        //when update topic state, change entry info anyway
        entryId: -1
      };
    case 'REQUEST_TOPIC_DETAIL':
      return {
        isLoading: true,
        message: 'loading...',
        topicId: action.topicId,

        ownerId: state.ownerId,
        topics: state.topics,
        //loading new topic info, clear old data
        entryId: -1
        //selectedTopic: state.selectedTopic,
        //selectedEntry: state.selectedEntry,
      };
    case 'RECEIVE_TOPIC_DETAIL':
      if (action.topic.id === state.topicId) {
        return {
          isLoading: false,
          message: action.message,
          selectedTopic: action.topic,

          ownerId: state.ownerId,
          topics: state.topics,
          topicId: state.topicId,
          entryId: -1
        };
      }
      break;
    case 'SELECT_TOPIC_ENTRY':
      //no local entry data, wait for recive_entry to update, or just hide
      return {
        entryId: state.entryId === action.entryId ? -1 : action.entryId,

        ownerId: state.ownerId,
        isLoading: state.isLoading,
        message: state.message,
        topics: state.topics,
        topicId: state.topicId,
        selectedTopic: state.selectedTopic
      };
    case 'REQUEST_ENTRIY_DETAIL': {
      return {
        isLoading: true,
        message: 'loading...',
        entryId: action.entryId,

        topicId: state.topicId,
        ownerId: state.ownerId,
        topics: state.topics,
        selectedTopic: state.selectedTopic
        //loading new topic info, clear old data
        //selectedTopic: state.selectedTopic,
        //selectedEntry: state.selectedEntry,
      };
    }
    case 'RECEIVE_ENTRIE_DETAIL': {
      if (action.entry.id === state.entryId) {
        return {
          isLoading: false,
          message: action.message,
          selectedEntry: action.entry,

          ownerId: state.ownerId,
          topics: state.topics,
          topicId: state.topicId,
          entryId: state.entryId,
          selectedTopic: state.selectedTopic
        };
      }
      break;
    }
    case 'POST_ENTRY_DETAIL':
    case 'RECEIVE_POST_ENTRIE_DETAIL':
      return {
        isLoading: false,
        message: '',
        ownerId: state.ownerId,
        topics: state.topics,
        topicId: state.topicId,
        entryId: state.entryId,
        selectedEntry: state.selectedEntry,
        selectedTopic: state.selectedTopic
      };
    default:
      // The following line guarantees that every action in the KnownAction union has been covered by a case above
      const exhaustiveCheck: never = action;
      break;
  }

  return state || unloadedState;
};