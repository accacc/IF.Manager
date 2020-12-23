﻿using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;

namespace IF.Manager.Service
{
    public class ModelClass: CSClass
    {

        
        IFModel model;
        
        public ModelClass(string nameSpace,string name,IFModel model)
        {            
            this.model = model;
            this.Name = $"{name}";
            this.NameSpace = nameSpace;            
        }


        public void Build(ModelClassTreeDto entityTree)
        {
            this.Usings.Add("System");
            this.Usings.Add("System.Collections.Generic");

            foreach (var childEntityTree in entityTree.Childs)
            {               
                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(childEntityTree,model);

                if (IsModelProperty)
                {
                    string name = childEntityTree.Name;
                    string type = childEntityTree.Type;

                    if (childEntityTree.IsRelation)
                    {
                        name = DirectoryHelper.AddAsLastWord(childEntityTree.Name, "DataModel");
                        name = $"{name}";
                        type = name;

                        if(childEntityTree.IsList)
                        {
                            type = $"IEnumerable<{name}>";
                        }
                    }

                    bool IsNullable = childEntityTree.IsNullable;
                    if (childEntityTree.Type == "string")
                    {
                        IsNullable = false;
                    }

                    var p = new CSProperty("public", name, IsNullable);
                    p.PropertyTypeString = type;
                    this.Properties.Add(p);
                }

            }

        }

        
    }
}
