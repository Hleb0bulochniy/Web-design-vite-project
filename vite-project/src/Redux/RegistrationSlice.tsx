import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface RegistrationState {
  isLoading: boolean;
  error: string | null;
}

const initialState: RegistrationState = {
  isLoading: false,
  error: null,
};



const registrationSlice = createSlice({
  name: 'registration',
  initialState,
  reducers: {
    registerRequest(state) {
      state.isLoading = true;
      state.error = null;
    },
    registerSuccess(state) {
      state.isLoading = false;
    },
    registerFailure(state, action: PayloadAction<string>) {
      state.isLoading = false;
      state.error = action.payload;
    },
  },
});

export const { registerRequest, registerSuccess, registerFailure } = registrationSlice.actions;

export default registrationSlice.reducer;