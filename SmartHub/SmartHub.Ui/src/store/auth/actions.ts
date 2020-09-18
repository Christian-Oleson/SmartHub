import { ActionContext, ActionTree } from 'vuex';
import { LoginRequest, AuthResponse, RegistrationRequest, UserUpdateRequest } from '@/types/types';
import { RootState, AuthState } from '@/store/index.types';
import { storeAuthResponse } from '@/services/auth/authService';
import { AuthMutations, M_AUTH, M_ME } from '@/store/auth/mutations';
import { postLogin, postRegistration, getMe, putMe } from '@/services/apis/identity.service';

// Keys
export const A_ME = 'A_ME';
export const A_UPDATE_ME = 'A_UPDATE_ME';
export const A_LOGIN = 'A_LOGIN';
export const A_REGISTRATION = 'A_REGISTRATION';
export const A_LOGOUT = 'A_LOGOUT';

// Actions
type AugmentedActionContext = {
  commit<K extends keyof AuthMutations>(key: K, payload: Parameters<AuthMutations[K]>[1] | null): ReturnType<AuthMutations[K]>;
} & Omit<ActionContext<AuthState, RootState>, 'commit'>;

// Action Interface
export interface AuthActions {
  [A_ME]({ commit }: AugmentedActionContext): Promise<void>;
  [A_UPDATE_ME]({ commit }: AugmentedActionContext, payload: UserUpdateRequest): Promise<void>;
  [A_LOGIN]({ commit }: AugmentedActionContext, payload: LoginRequest): Promise<void>;
  [A_REGISTRATION](state: AugmentedActionContext, payload: RegistrationRequest): Promise<void>;
  [A_LOGOUT]({ commit }: AugmentedActionContext): void;
}

// Define Actions
export const actions: ActionTree<AuthState, RootState> & AuthActions = {
  async [A_ME]({ commit }): Promise<void> {
    await getMe()
      .then((response) => {
        if (!response.success) {
          return Promise.reject(response.message);
        }
        commit(M_ME, response.data);
        return Promise.resolve(response.data);
      })
      .catch((err) => {
        console.log(err);
        return Promise.reject(err);
      });
  },
  async [A_UPDATE_ME]({ commit }, payload: UserUpdateRequest): Promise<void> {
    await putMe(payload)
      .then((response) => {
        if (!response.success) {
          return Promise.reject(response.message);
        }
        commit(M_ME, response.data);
        return Promise.resolve(response.data);
      })
      .catch((err) => {
        console.log(err);
        return Promise.reject(err);
      });
  },
  async [A_LOGIN]({ commit }, payload: LoginRequest): Promise<void> {
    await postLogin(payload)
      .then((response) => {
        if (!response.success) {
          return Promise.reject(response.message);
        }
        storeAuthResponse(response.data as AuthResponse);
        commit(M_AUTH, response.data);
        return Promise.resolve(response.data);
      })
      .catch((err) => {
        console.log(err);
        return Promise.reject(err);
      });
  },
  async [A_REGISTRATION](state, payload: RegistrationRequest): Promise<void> {
    await postRegistration(payload)
      .then((response) => {
        if (!response.success) {
          return Promise.reject(response.message);
        }
        storeAuthResponse(response.data as AuthResponse);
        state.commit(M_AUTH, response.data);
        return Promise.resolve();
      })
      .catch((err) => {
        console.log(err);
        return Promise.reject(err);
      });
  },
  async [A_LOGOUT]({ commit }) {
    console.log('logout');
  }
};
