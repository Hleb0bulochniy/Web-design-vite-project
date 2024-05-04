import 'bootstrap/dist/css/bootstrap.min.css';
import '../Card.css';
import { useAppDispatch, useAppSelector } from './Redux/Hooks';
import { BrowserRouter, Navigate, Route, Routes, } from 'react-router-dom';
import { ErrorPage } from './Pages/ErrorPage';
import { HomePage } from './Pages/HomePage';
import { NavigationPage } from './Pages/NavigationPage';
import { AuthPage } from './Pages/AuthPage';
import { RegisterPage } from './Pages/RegisterPage';
import { useEffect } from 'react';
import { logOut, loginPlease } from './Redux/AuthSlice';
import QuestionPage from './Pages/QuestionsPage';
import { AboutPage } from './Pages/AboutPage';
import { ContactsPage } from './Pages/ContactsPage';
import { CartPage } from './Pages/CartPage';

export interface webItem {
  Id: number;
  Name: string;
  Price: string;
  Image: string;
  Description: string;
  isBought: boolean;
}

function App() {
  const login = useAppSelector((state) => state.auth.isLogin);
  const dispatch = useAppDispatch();

  useEffect(() => {
    async function fetchData() {
      try {
        await fetch('https://localhost:7287/checkAuth', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
          },
        });
        console.log(login);
        dispatch(loginPlease());
      } catch (error) {
        dispatch(logOut());
        console.log(login);
      }
    }
    console.log(localStorage.getItem("accessToken"));

    fetchData();
  }, []);

  return (
    <BrowserRouter>
      <NavigationPage />
      <Routes>
        <Route path='*' element={<Navigate to="/home" />} />
        <Route path='/' element={<Navigate to="/home" />} />
        <Route path='/home' element={<HomePage />} />
        <Route path='/login' element={<AuthPage />} />
        <Route path='/registration' element={<RegisterPage />} />
        <Route path='/error' element={<ErrorPage />} />
        <Route path="/error/:id" element={<ErrorPage />} />
        <Route path='/questions' element={<QuestionPage />} />
        <Route path='/about' element={<AboutPage />} />
        <Route path='/contacts' element={<ContactsPage />} />
        <Route path='/cart' element={<CartPage />} />
      </Routes>
    </BrowserRouter>
  )
}



export default App