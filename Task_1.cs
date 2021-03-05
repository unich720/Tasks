using System;
using System.Collections.Generic;
using System.Text;

namespace test
{
    ///<summary>
    /// Пустрой интерфейс для контроля типов обобщенного интерфейса
    ///</summary>
    public interface IPath
    {
    }
    ///<summary>
    /// Обобщенный интерфейс для единиц измерения расстояния 
    ///</summary>
    public interface IPath<T> : IPath
        where T : struct, IPath
    {
        ///<summary>
        /// Свойство выражающее количество едениц измерения
        ///</summary>
        ///<value> 
        /// Свойство Path возвращающий значение в double 
        ///</value>
        double Path { get; }
        ///<summary> 
        /// Метод Add принимающий обобщенный тип добавляющий единицу измерения расстояния в другую
        ///</summary>
        void Add(T path);
    }
    ///<summary>
    /// Структура километров для перевода из миль в км
    ///</summary>
    public struct KmPAth : IPath<MilesPath>
    {
        ///<summary>
        /// Свойство выражающее количество едениц измерения
        ///</summary>
        ///<value> 
        /// Свойство Path возвращающий значение в double и принимающий значение
        ///</value>
        public double Path { get; private set; }
        ///<summary>
        /// Переменная для перевода миль в км
        ///</summary>
        private double KmOnMiles { get; set; }
        ///<summary>
        /// Конструктор километров 
        ///</summary>
        ///<param name="path">
        /// Количесво единиц измерения км
        ///</param>
        public KmPAth(double path)
        {
            Path = path;
            KmOnMiles = 0.6214;
        }
        ///<summary>
        /// Метод добавления милий к км
        ///</summary>
        ///<param name="path">
        /// Структура миль
        ///</param>
        public void Add(MilesPath path)
        {
            Path = path.Path * KmOnMiles;
        }
    }
    ///<summary>
    /// Структура миль для перевода из км в мили
    ///</summary>
    public struct MilesPath : IPath<KmPAth>
    {
        ///<summary>
        /// Свойство выражающее количество едениц измерения
        ///</summary>
        ///<value> 
        /// Свойство Path возвращающий значение в double и принимающий значение
        ///</value>
        public double Path { get; private set; }
        ///<summary>
        /// Переменная для перевода км в миль
        ///</summary>
        private double MilesOnKm { get; set; }
        ///<summary>
        /// Конструктор миль 
        ///</summary>
        ///<param name="path">
        /// Количесво единиц измерения миль
        ///</param>
        public MilesPath(double path)
        {
            Path = path;
            MilesOnKm = 1.609344;
        }
        ///<summary>
        /// Метод добавления км к милям
        ///</summary>
        ///<param name="path">
        /// Структура км
        ///</param>
        public void Add(KmPAth path)
        {
            Path = path.Path * MilesOnKm;
        }
    }
    ///<summary>
    /// Метод сохраняющий единицы измерения в БД
    ///</summary>
    public static void SavePathToDB<T>(string cargoID, IPath<T> path)
            where T : struct, IPath
    {

    }
}
