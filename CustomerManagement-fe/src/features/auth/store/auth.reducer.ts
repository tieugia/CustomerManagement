import { createReducer, on } from "@ngrx/store";
import { AuthState } from "./auth.state";
import * as AuthActions from './auth.actions';

export const initialState: AuthState = {
    token: null,
    loading: false,
    error: null,
  };

export const authReducer = createReducer(
    initialState,
  
    on(AuthActions.createLoginAction, state => ({
      ...state,
      loading: true,
      error: null
    })),
  
    on(AuthActions.loginSuccess, (state, { token }) => ({
      ...state,
      token,
      loading: false
    })),
  
    on(AuthActions.loginFailure, (state, { error }) => ({
      ...state,
      loading: false,
      error
    })),

    on(AuthActions.logout, () => ({
        ...initialState
      }))
    );