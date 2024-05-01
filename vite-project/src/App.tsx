import 'bootstrap/dist/css/bootstrap.min.css';
import { Button1 } from './Components/button1';
import { useEffect, useState } from 'react';
import Card1 from './Components/Card';
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
import axios from 'axios';
import { productApi } from './Api/Api';

export interface webItem {
  id: number;
  name: string;
  price: string;
  image: string;
  description: string;
  isBought: boolean;
}

function App() {
  const [data, setData] = useState<webItem[]>([]);
  const s = useAppSelector((state) => state.counter.value);
  const login = useAppSelector((state) => state.auth.isLogin);
  const token = localStorage.getItem('accessToken');


  /*useEffect(() => {
    fetch('https://localhost:7287/web2additem', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    })
      .then(res => res.json())
      .then(json => setData(json))
  }, [])*/

    /*useEffect(() => {
    axios.get('https://localhost:7287/web2additem', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    })
      .then(res => {console.log(res); setData(res.data)})
  }, [])*/
  useEffect(() => {
    productApi("web2additem").then(res => {console.log(res); setData(res.data)})
  }, [])




  const handleBuyClick = (itemId: number) => {
    setData(data.map(item =>
      item.id === itemId ? { ...item, isBought: !item.isBought } : item
    ));
  };

  return (
    <BrowserRouter>
    <NavigationPage/>
      <div className="card-container">
        {data?.map((item: webItem) => (
          <div key={item.id} className="card-item">
            <Card1
              name={item.name}
              description={item.description}
              image={item.image}
              price={item.price}
              button={<Button1 state={item.isBought} fun={() => handleBuyClick(item.id)} textBuy={item.price} textBought='Добавлено!' />}
            />
          </div>
        ))}
      </div>
      <Button2 />
      <h1>{s}</h1>
      <Auth />
      <h1>{login}</h1>
      <ButtonReg />
      <Check />
      <Routes>
        <Route path='*' element={<Navigate to="/home" />} />
        <Route path='/' element={<Navigate to="/home" />} />
        <Route path='/home' element={<HomePage />} />
        <Route path='/error' element={<ErrorPage />} />
        <Route path="/error/:id" element={<ErrorPage />} />
      </Routes>



    </BrowserRouter>
  )
}



export default App