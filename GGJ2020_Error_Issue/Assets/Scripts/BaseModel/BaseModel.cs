using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using UniRx;

namespace Models
{
    abstract public class BaseModel : MonoBehaviour
    {
        public class Model
        {
            public int id;

            public List<FloatReactiveProperty> floatAttrs = new List<FloatReactiveProperty>();
            public List<IntReactiveProperty> intAttrs = new List<IntReactiveProperty>();
            public List<StringReactiveProperty> stringAttrs = new List<StringReactiveProperty>();

            protected void Register(IntReactiveProperty attr)
            {
                intAttrs.Add(attr);
            }
            protected void Register(FloatReactiveProperty attr)
            {
                floatAttrs.Add(attr);
            }
            protected void Register(StringReactiveProperty attr)
            {
                stringAttrs.Add(attr);
            }

            virtual protected void InitInstance() { }

            virtual protected void RegisterAttributes() { }

            protected string modelName
            {
                get { return this.GetType().ToString().Split(new char[] { '+' })[0]; }
            }

            public Model()
            {
                InitInstance();
                RegisterAttributes();

                floatAttrs.ForEach(a =>
                {
                    a.AsObservable()
                        .Skip(1)
                        .Do(_ => Debug.Log(_))
                        .Do(value => PlayerPrefs.SetFloat(modelName + floatAttrs.IndexOf(a).ToString() + "float" + id, value))
                        .Subscribe(_ => PlayerPrefs.Save());
                });
                intAttrs.ForEach(a =>
                {
                    a.AsObservable()
                        .Skip(1)
                        .Do(_ => Debug.Log(_))
                        .Do(value => PlayerPrefs.SetInt(modelName + intAttrs.IndexOf(a).ToString() + "int" + id, value))
                        .Subscribe(_ => PlayerPrefs.Save());
                });
                stringAttrs.ForEach(a =>
                {
                    a.AsObservable()
                        .Skip(1)
                        .Do(_ => Debug.Log(_))
                        .Do(value => PlayerPrefs.SetString(modelName + stringAttrs.IndexOf(a).ToString() + "string" + id, value))
                        .Subscribe(_ => PlayerPrefs.Save());
                });

                PlayerPrefs.SetInt(modelName + "count", id + 1);
            }
        }

        virtual protected Model Instantiate()
        {
            return new Model();
        }

        public void Awake()
        {
            string modelName = this.GetType().ToString();

            int instanceCount = PlayerPrefs.HasKey(modelName + "count") ? PlayerPrefs.GetInt(modelName + "count") : 0;
            Enumerable.Range(0, instanceCount).ToList().ForEach(i =>
            {
                Model instance = Instantiate();

                instance.floatAttrs.ForEach(a =>
                {
                    a.Value = PlayerPrefs.GetFloat(modelName + instance.floatAttrs.IndexOf(a).ToString() + "float" + instance.id);
                });
                instance.intAttrs.ForEach(a =>
                {
                    a.Value = PlayerPrefs.GetInt(modelName + instance.intAttrs.IndexOf(a).ToString() + "int" + instance.id);
                });
                instance.stringAttrs.ForEach(a =>
                {
                    a.Value = PlayerPrefs.GetString(modelName + instance.stringAttrs.IndexOf(a).ToString() + "string" + instance.id);
                });
            });
        }
    }
}