import { PayloadAction, createSlice } from "@reduxjs/toolkit"

export interface AuthState {
  isLoading: boolean;
  isError: boolean;
  isLogin: boolean;
  aToken: Token;
  rToken: Token;
  message: string;
}

export interface Token {
  isPresent: boolean;
  token: string;
}

const initialState: AuthState = {
  isError: false,
  isLoading: false,
  isLogin: false,
  aToken: {
    isPresent: false,
    token: ""
  },
  rToken: {
    isPresent: false,
    token: ""
  },
  message: ""
};

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    login: (state, action: PayloadAction<{ accessToken: string; refreshToken: string; username: string }>) => {
      state.isLoading = false;
      state.isError = false;
      state.isLogin = true;
      state.aToken.isPresent = true;
      state.aToken.token = action.payload.accessToken;
      state.rToken.isPresent = true;
      state.rToken.token = action.payload.refreshToken;
    },
    loginRequest: (state) => {
      state.isLoading = true;
      state.isError = false;
    },
    loginFailure: (state, action: PayloadAction<string>) => {
      state.isLoading = false;
      state.isError = true;
      state.message = action.payload;
    },
    logOut: (state) => {
      state.isLoading = false;
      state.isError = false;
      state.isLogin = false;
      state.aToken.isPresent = false;
      state.aToken.token = "";
      state.rToken.isPresent = false;
      state.rToken.token = "";
      localStorage.setItem('accessToken', "");
      console.log("логин "+state.isLogin);
    },
    loginPlease: (state) => {
      state.isLoading = false;
      state.isError = false;
      state.isLogin = true;
      state.aToken.isPresent = true;
      state.aToken.token = localStorage.getItem("accessToken")!;
      state.rToken.isPresent = true;
      state.rToken.token = "";
      console.log("ЛОГИН "+state.isLogin);
    }
  }
})

export const { login, loginRequest, loginFailure, logOut, loginPlease } = authSlice.actions