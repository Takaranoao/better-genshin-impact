using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace BetterGenshinImpact.Core.Recognition.OpenCv.FeatureMatch;

public class YamlVectorOfKeyPointConverter : IYamlTypeConverter
{
    public static readonly YamlVectorOfKeyPointConverter Instance = new();

    public bool Accepts(Type type)
    {
        return type == typeof(VectorOfKeyPoint);
    }

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        parser.Consume<MappingStart>();
        var key = parser.Consume<Scalar>().Value;
        if (key != "kp")
        {
            throw new YamlException($"Expected key 'kp', but got '{key}'");
        }

        parser.Consume<SequenceStart>();
        var keyPointList = new List<MKeyPoint>();
        while (parser.TryConsume<SequenceStart>(out _))
        {
            var keyPoint = new MKeyPoint();
            var x = parser.Consume<Scalar>().Value;
            var y = parser.Consume<Scalar>().Value;
            keyPoint.Point = new PointF(float.Parse(x), float.Parse(y));
            var size = parser.Consume<Scalar>().Value;
            keyPoint.Size = float.Parse(size);
            var angle = parser.Consume<Scalar>().Value;
            keyPoint.Angle = float.Parse(angle);
            var response = parser.Consume<Scalar>().Value;
            keyPoint.Response = float.Parse(response);
            var octave = parser.Consume<Scalar>().Value;
            keyPoint.Octave = int.Parse(octave);
            var classId = parser.Consume<Scalar>().Value;
            keyPoint.ClassId = int.Parse(classId);
            keyPointList.Add(keyPoint);
            parser.Consume<SequenceEnd>();
        }

        parser.Consume<MappingEnd>();
        return new VectorOfKeyPoint(keyPointList.ToArray());
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        var obj = (VectorOfKeyPoint)value!;
        emitter.Emit(new MappingStart());
        emitter.Emit(new Scalar("kp"));
        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Block));
        foreach (var keyPoint in obj.ToArray())
        {
            emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));
            emitter.Emit(new Scalar(keyPoint.Point.X.ToString(CultureInfo.InvariantCulture)));
            emitter.Emit(new Scalar(keyPoint.Point.Y.ToString(CultureInfo.InvariantCulture)));
            emitter.Emit(new Scalar(keyPoint.Size.ToString(CultureInfo.InvariantCulture)));
            emitter.Emit(new Scalar(keyPoint.Angle.ToString(CultureInfo.InvariantCulture)));
            emitter.Emit(new Scalar(keyPoint.Response.ToString(CultureInfo.InvariantCulture)));
            emitter.Emit(new Scalar(keyPoint.Octave.ToString()));
            emitter.Emit(new Scalar(keyPoint.ClassId.ToString()));
            emitter.Emit(new SequenceEnd());
        }

        emitter.Emit(new SequenceEnd());
        emitter.Emit(new MappingEnd());
    }
}