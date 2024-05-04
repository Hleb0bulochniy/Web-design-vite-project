import { configureStore } from '@reduxjs/toolkit'
import { counterSlice } from './CounterSlice'
import { authSlice } from './AuthSlice';
import { SumSlice } from './SumSlice';

export const store = configureStore({
  reducer: {
    counter: counterSlice.reducer,
    auth: authSlice.reducer,
    sum: SumSlice.reducer,
  },
});

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch