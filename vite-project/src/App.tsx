import 'bootstrap/dist/css/bootstrap.min.css';
import '../Card.css';
import { Button2 } from './Components/button2';
import { useAppSelector } from './Redux/Hooks';
import { Auth } from './Components/Auth';
import { ButtonReg } from './Components/Registration';
import { Check } from './Components/AuthCheck';
import { BrowserRouter, Navigate, Route, Routes, } from 'react-router-dom';
import { ErrorPage } from './Pages/ErrorPage';
import { HomePage } from './Pages/HomePage';
import { NavigationPage } from './Pages/NavigationPage';
import { StorePage } from './Pages/StorePage';

export interface webItem {
  Id: number;
  Name: string;
  Price: string;
  Image: string;
  Description: string;
  isBought: boolean;
}

function App() {
  const s = useAppSelector((state) => state.counter.value);
  const login = useAppSelector((state) => state.auth.isLogin);

  return (
    <BrowserRouter>
      <NavigationPage />

      <Button2 />
      <h1>{s}</h1>
      <Auth />
      <h1>{login}</h1>
      <ButtonReg />
      <Check />
      <Routes>
        <Route path='*' element={<Navigate to="/error" />} />
        <Route path='/' element={<Navigate to="/error" />} />
        <Route path='/home' element={<HomePage />} />
        <Route path='/error' element={<ErrorPage />} />
        <Route path="/error/:id" element={<ErrorPage />} />
      </Routes>
    </BrowserRouter>
  )
}



export default App